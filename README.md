# 交互全流程

此项目通过osc通信解决了多物质媒介之间信息传输的问题。

实现多媒介之间的交互主要解决两个问题，网络通信和坐标轴同步，网络通信可以保证每台设备与其他媒介之间进行数据交换，坐标轴同步可以保证每台设备中虚拟内容在同一时间同一位置出现。

在网络通信的解决方案上我们采用了keijiro的oscjack进行unity中的信息广播，再用td作为中控统一接收每台设备广播出的信息，通过本地路由器作为每个媒介数据传输的解决方案。
在坐标轴同步的解决方案中我们通过每台手机在td坐标系中拿到的位置和在unity中提前确认的坐标的数据反算出基于td坐标系的统一基准，实现跨媒介交互的位置时间同步。

整个交互流程图:

![image](https://github.com/holoi/warp-drive/blob/main/%E5%85%A8%E6%B5%81%E7%A8%8B.png)


## 软件环境
1.unity版本： 2022.3.11f1c1或以上的3D(URP)模版的IOS系统。

2.[Holokit SDK](https://github.com/holoi/holokit-sdk?tab=readme-ov-file#holokit-sdk)

3.AR Foundation，在Packagemanager里添加。

4.[OSC Jack](https://github.com/keijiro/OscJack) 需要在大多数平台上受支持，但只有少数平台受支持 网络限制平台，如 WebGL。

5.手机版本：iphone12pro及以上。

## 图像校准
基本原理：利用重力的方向确定Y轴的朝向，将变量约束在xz平面内。向该平面做投影，计算Unity中的虚拟定位图片与Camera拍摄到的现实定位图片的夹角，通过这个夹角进行整个空间的校准，使AR呈现的物象处在现实中的合理位置。

使用方法：在Settings中的Reference Image Library中只添加一张图片，同时将这张照片打印出来粘贴。现实定位图片的大小和位置应该与虚拟定位图片在Unity中的大小和位置完全相同。在启动应用后，将取景框/视线看向现实定位图片，出现校准的坐标轴后，点击Calibrate进行校准。

一次校准的误差大约在±10cm，由于后续运行也会不可避免地产生定位误差，因此建议进行3-5次的校准，在虚拟定位图片与现实定位图片几乎完全重合时再停止校准。

ARFoundation的Image Tracking功能对图像有一定要求，彩色的参考图使用黑白图片也可以追踪，大小要求见下表，橙色区域内效果较好。

![image](https://github.com/holoi/warp-drive/blob/main/Assets/WarpDrive/Textures/sec.png)


注意事项
本项目依赖Arkit，因此运行手机必须是iPhone。由于需要进行较为精确的定位，因此强烈建议安装在有LiDAR激光雷达的Pro系列设备上（iPhone 12 Pro及以上的Pro系列手机，iPad Pro 2018及以上Pro系列平板电脑）。

空间定位准确度主要受场地影响。Arkit几乎完全依赖LiDAR进行定位，视觉定位仅用于辅助。LiDAR的有效范围大概在10m左右，暗光环境也可以正常工作，但是在空旷空间这个距离会减少到约6-8m，超出有效范围开始依靠视觉定位进行辅助。LiDAR需要正常的反射物体表面才能进行定位，在大理石、混凝土、石膏等表面表现良好，吸光铺装表面几乎无法定位，容易漂移。

当LiDAR开始漂移时，视觉定位会开始主导，此时定位的效果会变差，但依然能大致进行定位。如果你的环境不适合使用LiDAR定位，请保证空间中没有频繁移动的物体和画面。在我们的项目测试中，在不同材质上使用时，LIDAR定位会出现误差，由于铺装了低反射地面导致LiDAR无法正常工作，系统转为视觉定位，此时若是投影内容不固定/有位移，Unity的AR内容会立刻开始漂移。



# TD与arkit中的osc信息传输
校准完位置信息后，将正确的手机位置传输给Touchdesigner，同时Touchdesigner传输给手机对应的时间信息触发对应模块。

数据的传输可通过手机热点、局域网络、本地网络进行连接，设备需要在同一网络之下，否则无法传输。数据的稳定性取决于网络环境是否优良，数据传输的距离。

哪里收信息填写哪里的IP，unity中OscConnectSender和Touchdesigner中的osc in对应，unity中OscConnectReceiver和Touchdesigner中的osc out对应，相互对应的二者IP、端口号必须一致。

unity中，你可以在Assets-WarpDrive-OSC中找到原始文件，两个空物体分别绑定OSCJack中的Property Sender和Event Receiver组件，并添加OSC Connection Sender/Receiver的UDP。

## OSC 属性发送方sender
Touchdesigner接收unity ARkit传来的信息，需要将unity中将发送信息的物体拖入Data Source,在Component中选取此物体带有的发送信息的组件，在Property中选择具体传输的数据。从unity传输不同信息可以添加多个组件，选取自己想发送给TD的信息。在本方案中将Holokit之下的Momo Camera拖入Data Source，已达到双目模式下的手机位置和TD展现出的同步。需要注意的是直接将Holokit XR Origin拖入Data Source传输位置信息可以在unity中测试成功，但无法在导入手机后传输手机的位置信息。


![image](https://github.com/holoi/warp-drive/blob/main/%E4%BA%A4%E4%BA%92.jpg)



如工程文件中：Component中，在Transform，BoxCollider，ParentConstraint里选择了Transform,以获取位置信息，Transform组件中又在下列具体选择：
position,localposition,eulerAngles,localEulerAngles,right,up,forward,localscale,chileCount,lossyScale,hierarchyCapacity,hierarchyCount,tag,name。最后手机传输了position和eulerAngles，TD最终接收的即为这两个信息。


![image](https://github.com/holoi/warp-drive/blob/main/%E5%B1%8F%E5%B9%95%E6%88%AA%E5%9B%BE3.png)


填写Touchdesigner所在主机的IP，端口号相同即可收到，在本案例中将unity中玩家的位置和旋转信息发送给了TD，在TD场景内显示玩家所在的实时位置，实现位置同步。在工程案例中通过select选择接收的数据并且赋值给TD中所代表ARkit的物体。


![image](https://github.com/holoi/warp-drive/blob/main/%E5%B1%8F%E5%B9%95%E6%88%AA%E5%9B%BE2.png)



TD使用Holokit的位置确定投影的交互效果产生的位置
（以3台手机设备为例）
td作为unity和cg的中枢，包含了三个数据块工程和一个中控台，每个数据块工程都包含了base1，base2，base3，每个数据工程和base的节点连接方法都类似，其原理都是通过oscin原本的和经过绿色节点加工过的数据来控制数据块的交互。osc捕捉手机的实时位置，其数据通过oscin传入td，3个手机设备就分别对应3个不同的oscin中的数据，分别为phone1，phone2，phone3，其中只需要关注分别对应的position数据。
position数据经过处理（连接后面的math，lag，local等绿色节点）连入相应的td工程节点。

（每个节点都一一对应）
如第一个工程中的base1

其中的parent(2).op('lag2')['phone1/position3']与osein连接的lag2相连

将数据都连接好之后，人移动的位置就会被Holokit捕捉到，传输给td，触发交互





## OSC 事件接收器receive
OSC 事件接收器接收 OSC 消息，并使用接收到的数据。
在我们这个案例里，unity和Touchdesigner之间相互传输消息。在unity接收Touchdesigner传来的信息时，填写unity所在主机的IP（unity应用于手机时填写手机端IP）且端口号相同即可收到信息。
在本项目中实现了屏幕的三个时空和Holokit所展示的三个场景的同步对应。
OSC 监视器
OSC Monitor 是一个用于检查传入 OSC 消息的小型实用程序。打开 监视器，导航到窗口> OSC 监视器。
如下方，unity接收到了TD传来的时间信息并显示在了OSC Monitor中。

运用接收到的TD信息：Holokit需要从TD同步时间轴，在指定时间开启相应的组件。

OSCAddress内填写OSC Monitor接收的信息名称（如上图在/second和/frame中选择了/second，选择Date Type的类型，用于脚本，以直接调用收到的数据）。


Receive form中，使用
public void ActivePortal()
声明好需要使用到的方法变量，例如，输出是Float，就填入（Float time），然后选择好对应的Data Type，就可以在** Event中找到对应的事件了。

新建一个空物体TimelineController并挂上OSC Timeline脚本，写好每一个时间段需要启用的组件，并绑定场景中的Portal组件。每一个Portal中都有三个Wall Collider，在脚本Wall Collider Trigger中检查他们的Tag，从而在不同的方向激活Portal VFX。

因为希望VFX粒子在举手之前呈现无序漂浮的状态，因此引入脚本Finger Control。只有在Camera中检测到Hand后，Finger才会被启动，VFX粒子才会转为跟随状态。

脚本Way Point Binder主要负责同步Finger的位置，当达到一定时间后停止同步，这样就可以被固定在空间中。脚本VFX Stream Control主要控制VFX粒子的走向，这些选项已经被暴露在了VFX粒子的Inspector中，因此可以被代码读取和控制。



第三人称观看和录制
如需第三人称观看和录制，需要一位玩家的Head和Finger的位置信息，因此无需发送任何信息，只需要接收来自TD的另一台设备的Head和Finger的位置即可。








# VFX节点和效果截图需要的URP管线
![image](https://github.com/holoi/warp-drive/blob/main/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20231224165706.png)

![image](https://github.com/holoi/warp-drive/blob/main/urp%E7%AE%A1%E7%BA%BF.png)


vfx节点截图
![image](https://github.com/holoi/warp-drive/blob/main/urp.png)




