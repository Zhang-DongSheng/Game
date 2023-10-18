# Game
  Unity 工具类项目

##**UIFramework**
+ UImanager
+ CtrlBase
+ UIBase
```javascript
private void Open() {
    UIManager.Instance.Open(UIPanel.UILotteryDraw);
}
```
##**Runtime**
+ RuntimeManager
+ RuntimeEvent
+ RuntimeBehaviour

Unity生命周期管理，通过反射判断子类是否重写生命周期函数自动注册

##**事件派发器**
- EventManager
- EventKey
- EventMessageArgs
```javascript
private void Send() {
  EventMessageArgs args = new EventMessageArgs();
  args.AddOrReplace("a", 1);
  args.AddOrReplace("b", "2");
  EventManager.Post(EventKey.Login, args);
}
```
##**资源下载**
+ RenewableResource `独立资源下载`
+ ResourceManager `资源下载管理器`
```javascript
public void LoadPrefb(float index) {
  ResourceManager.Initialize(LoadingType.AssetBundle);
  var prefab = ResourceManager.Load<GameObject>("Res/xx.prefab");
  var go = GameObject.Instantiate(prefab);
}
```
##**网络消息**
+ Client
    + AsyncNetworkClient
    + NetworkClient

##**动画模块**
+ Animation

##**扩展方法**
+ Extension

##**工具类**
+ Utility

##**IFix使用说明**
>1. 在 github 下载源码工程，【https://github.com/Tencent/InjectFix】
>2. 编译 IFix.exe（后续生成 patch 和对原始代码插桩都要用到）
Windows 下打开 Source\VSProj\build_for_unity.bat，将 UNITY_HOME 变量的值修改为指向本机 unity 安装目录，运行 build_for_unity.bat；
Mac 下打开 Source\VSProj\build_for_unity.sh，将 UNITY_HOME 变量的值修改为指向本机 unity 安装目录，运行 build_for_unity.sh；
查看是否生成 Source\UnityProj\IFixToolKit\IFix.exe；
>3. 用 unity 打开工程 Source\UnityProj
>4. 打开场景 Assets\Helloworld\Helloworld.unity，此时运行游戏是错误函数的逻辑
```javascript
//Calc.cs
public int Add(int a, int b) {
  return a * b;
}
```
输出：10 + 9 = 90
>5. 修改代码，并添加 patch 标签
```javascript
//Calc.cs
[Patch]
public int Add(int a, int b) {
  return a + b;
}
```
>6. 点击菜单【InjectFix/Fix】，生成 patch 文件，注意默认生成在工程的根目录下，拷贝 patch 文件到 Assets\Resources 目录下，因为 helloworld 测试用的是 Resources.Load
>7. 将 5 修改的代码还原为 4 的原始代码，待编译完成，此时游戏的代码应该还是错误的
>8. 点击菜单【InjectFix/Inject】，对代码插桩
>9. 启动游戏，查看结果，修复完成，输出：10 + 9 = 19

联系邮箱 ：202689420@qq.com