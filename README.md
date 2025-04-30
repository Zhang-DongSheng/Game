# Game
  Unity 工具类项目

##**UIFramework**
+ UImanager
+ CtrlBase
+ UIBase
+ ItemBase
```javascript
private void Open()
{
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
private void Send()
{
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
public void LoadPrefb(float index)
{
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

联系邮箱 ：202689420@qq.com