// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileContributor: Botao Amber Hu <botao@holoi.com>
// SPDX-License-Identifier: MIT

#include <TargetConditionals.h>

#if TARGET_OS_IOS
#import <UIKit/UIKit.h>
#endif

#import <AVFoundation/AVFoundation.h>
#import <Metal/Metal.h>
#include <CoreMedia/CMBlockBuffer.h>
#import <Accelerate/Accelerate.h>

static AVAssetWriter* _writer;
static AVAssetWriterInput* _videoWriterInput;
static AVAssetWriterInput* _audioWriterInput;
static AVAssetWriterInputPixelBufferAdaptor* _bufferAdaptor;
static AudioStreamBasicDescription _audioFormat;
static CMFormatDescriptionRef _cmFormat;

static bool _isRecording;
static int _width;
static int _height;

extern "C" {

int HoloKitVideoRecorder_StartRecording(const char* filePath, int width, int height,
                                        float audioSampleRate, int audioChannelCount) {
    if (_writer)
    {
        NSLog(@"Recording has already been initiated.");
        return -1;
    }
    
    // Asset writer setup
    NSURL* filePathURL = [NSURL fileURLWithPath:[NSString stringWithUTF8String:filePath]];
    
    NSError* err;
    _writer = [[AVAssetWriter alloc] initWithURL: filePathURL
                                        fileType: AVFileTypeMPEG4
                                           error: &err];
    
    if (err)
    {
        NSLog(@"Failed to initialize AVAssetWriter (%@)", err);
        return -1;
    }
    
    NSDictionary* settings = @{ 
        AVVideoCodecKey: AVVideoCodecTypeHEVC,
        AVVideoWidthKey: @(width),
        AVVideoHeightKey: @(height) };
    
    _videoWriterInput = [AVAssetWriterInput assetWriterInputWithMediaType: AVMediaTypeVideo outputSettings: settings];
    _videoWriterInput.expectsMediaDataInRealTime = YES;

    // Pixel buffer adaptor setup
    NSDictionary* attribs = @{ 
        (NSString*)kCVPixelBufferPixelFormatTypeKey: @(kCVPixelFormatType_32BGRA),
        (NSString*)kCVPixelBufferWidthKey: @(width),
        (NSString*)kCVPixelBufferHeightKey: @(height) };
    
    _bufferAdaptor = [AVAssetWriterInputPixelBufferAdaptor
                      assetWriterInputPixelBufferAdaptorWithAssetWriterInput: _videoWriterInput
                      sourcePixelBufferAttributes: attribs];
    
    // Audio writer input setup
    NSDictionary* audioSettings = @{
        AVFormatIDKey: @(kAudioFormatMPEG4AAC),
        AVSampleRateKey: @(audioSampleRate),
        AVNumberOfChannelsKey: @(audioChannelCount),
        AVEncoderAudioQualityKey: @(AVAudioQualityHigh)
    };
    _audioWriterInput = [AVAssetWriterInput assetWriterInputWithMediaType:AVMediaTypeAudio
                         outputSettings:audioSettings];
    _audioWriterInput.expectsMediaDataInRealTime = YES;
    
    _audioFormat.mSampleRate = audioSampleRate; // Sample rate, 44100Hz is CD quality
    _audioFormat.mFormatID = kAudioFormatLinearPCM; // Specify the data format to be PCM
    _audioFormat.mFormatFlags = kLinearPCMFormatFlagIsFloat; // Flags specific for the format
    _audioFormat.mFramesPerPacket = 1; // Each packet contains one frame for PCM data
    _audioFormat.mChannelsPerFrame = (uint32_t) audioChannelCount; // Set the number of channels
    _audioFormat.mBitsPerChannel = sizeof(float) * 8; // Number of bits per channel, 32 for float
    _audioFormat.mBytesPerFrame = (uint32_t) audioChannelCount * sizeof(float); // Bytes per frame
    _audioFormat.mBytesPerPacket = _audioFormat.mBytesPerFrame * _audioFormat.mFramesPerPacket; // Bytes per packet
    CMAudioFormatDescriptionCreate(kCFAllocatorDefault,
                               &_audioFormat,
                               0,
                               NULL,
                               0,
                               NULL,
                               NULL,
                               &_cmFormat
                               );

    [_writer addInput:_videoWriterInput];
    [_writer addInput:_audioWriterInput];
    
    // Recording start
    if (![_writer startWriting])
    {
        NSLog(@"Failed to start (%ld: %@)", _writer.status, _writer.error);
        return -1;
    }

    _width = width;
    _height = height;
    [_writer startSessionAtSourceTime:kCMTimeZero];
    _isRecording = YES;  

    return 0;
}

int HoloKitVideoRecorder_AppendAudioFrame(void* source, int size, double time)
{
    if (!_isRecording) {
        return -2;
    }

    if (!_writer)
    {
        NSLog(@"Recording hasn't been initiated.");
        return -1;
    }

    if (!_audioWriterInput.isReadyForMoreMediaData)
    {
        NSLog(@"Audio Writer is not ready.");
        return -1;
    }

    // Write _audioInputWriter with buffer 

    CMTime presentationTimestamp = CMTimeMakeWithSeconds(time, 240); // Adjust timescale as needed
    
    CMBlockBufferRef blockBuffer;
    OSStatus status = CMBlockBufferCreateWithMemoryBlock(kCFAllocatorDefault,
                                                         source,
                                                         size, kCFAllocatorNull, NULL, 0, size, kCMBlockBufferAssureMemoryNowFlag, &blockBuffer);
    
    if (status != noErr) {
        NSLog(@"CMBlockBufferCreateWithMemoryBlock error");
        return -1;
    }

    int nSamples = size / _audioFormat.mBytesPerFrame;

     AudioStreamBasicDescription audioFormat = *CMAudioFormatDescriptionGetStreamBasicDescription(_audioFormat);

    CMSampleBufferRef sampleBuffer;
    status = CMAudioSampleBufferCreateReadyWithPacketDescriptions(kCFAllocatorDefault,
                                                             blockBuffer,
                                                             presentationTimestamp, _cmFormat, &sampleBuffer);
                                                             TRUE,
                                                             NULL,
                                                             NULL,
                                                             _cmFormat,
                                                             nSamples,
                                                             presentationTimestamp,
                                                             NULL,
                                                             &sampleBuffer);
    
    if (status != noErr) {
        CFRelease(blockBuffer);
        return -1;
    }

    if (!CMSampleBufferDataIsReady(sampleBuffer))
    {
        NSLog(@"sample buffer is not ready");
        return -1;
    }
    if (!CMSampleBufferIsValid(sampleBuffer))
    {
        NSLog(@"Audio sapmle buffer is not valid");
        return -1;
    }
    
    status = CMSampleBufferMakeDataReady(sampleBuffer);
    if (status == noErr) {
        [_audioWriterInput appendSampleBuffer:sampleBuffer];
        return -1;
    }
    
    CFRelease(sampleBuffer);
    CFRelease(blockBuffer);
    return 0;
}

int HoloKitVideoRecorder_AppendVideoFrame(const char* source, uint32_t size, double time)
{
    if (!_isRecording) {
        return -2;
    }

    if (!_writer)
    {
        NSLog(@"Recording hasn't been initiated.");
        return -1;
    }
    
    if (!_videoWriterInput.isReadyForMoreMediaData)
    {
        NSLog(@"Video Writer is not ready.");
        return -1;
    }
    
    if (!_bufferAdaptor.pixelBufferPool)
    {
        NSLog(@"Video Writer pixelBufferPool is empty.");
        return -1;
    }

    // Buffer allocation
    CVPixelBufferRef pixelBuffer = nil;
    CVReturn ret = CVPixelBufferPoolCreatePixelBuffer(NULL, _bufferAdaptor.pixelBufferPool, &pixelBuffer);
    
    if (ret != kCVReturnSuccess)
    {
        NSLog(@"Can't allocate a pixel buffer (%d)", ret);
        NSLog(@"%ld: %@", _writer.status, _writer.error);
        return -1;
    }
    
    // Buffer copy
    CVPixelBufferLockBaseAddress(pixelBuffer, 0);
    void* baseAddress = CVPixelBufferGetBaseAddress(pixelBuffer);
    int bytesPerRow = CVPixelBufferGetBytesPerRow(pixelBuffer);
    int bufferSize = CVPixelBufferGetDataSize(pixelBuffer);
   
    vImage_Buffer srcBuffer;
    srcBuffer.data = (void*) source;
    srcBuffer.height = _height;
    srcBuffer.width = _width;
    srcBuffer.rowBytes = _width * 4;

    vImage_Buffer dstBuffer;
    dstBuffer.data = baseAddress;
    dstBuffer.height = _height;
    dstBuffer.width = _width;
    dstBuffer.rowBytes = bytesPerRow;

    const uint8_t permuteMap[4] = { 2, 1, 0, 3 };
    assert(size == _width * _height * 4);
    vImagePermuteChannels_ARGB8888(&srcBuffer, &dstBuffer, permuteMap, kvImageNoFlags);
    CVPixelBufferUnlockBaseAddress(pixelBuffer, 0);

    // Buffer submission
    [_bufferAdaptor appendPixelBuffer:pixelBuffer
                    withPresentationTime:CMTimeMakeWithSeconds(time, 240)];
    
    CVPixelBufferRelease(pixelBuffer);

    return 0;
}

void HoloKitVideoRecorder_EndRecording(void)
{
    if (!_isRecording) {
        return;
    }

    if (!_writer)
    {
        NSLog(@"Recording hasn't been initiated.");
        return;
    }
    
    [_videoWriterInput markAsFinished];
    [_audioWriterInput markAsFinished];
    
#if TARGET_OS_IOS
    NSString* path = _writer.outputURL.path;
    [_writer finishWritingWithCompletionHandler: ^{
        if (UIVideoAtPathIsCompatibleWithSavedPhotosAlbum(path)) {
            UISaveVideoAtPathToSavedPhotosAlbum(path, nil, nil, nil);
        }
    }];
#else
    [_writer finishWritingWithCompletionHandler: ^{}];
#endif
    _writer = NULL;
    _videoWriterInput = NULL;
    _audioWriterInput = NULL;
    _bufferAdaptor = NULL;
    _isRecording = NO;
}

}
