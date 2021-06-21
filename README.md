# Game
  Unity 工具类项目

##UI模块
+ UImanager `UI管理器`
+ Config `配置`
+ Paramter `参数`
+ CtrlBase `控制基类`
+ UIBase : MonoBehaviour `UI基类`
  - UIMain
  - UILogin
  - UILoading
  - UIWaiting
  - UIConfirm
  - UINotice
  - UIReward
  - UILotteryDraw
```javascript
private void Open() {
    UIManager.Instance.Open(UIPanel.UILotteryDraw);
}
```

##事件通知模块
- EventManager `派发器`
- EventMessageArgs `事件参数`
- EventKey `索引`
```javascript
private void Send() {
    EventMessageArgs args = new EventMessageArgs();
    args.AddOrReplace("a", 1);
    args.AddOrReplace("b", "2");
    EventManager.Post(EventKey.Login, args);
}
```

##网络消息
+ Client `客户端`
    + AsyncNetworkClient `异步连接`
    + NetworkClient `同步`

##动画模块 SAM (Sample Animation)
+ SAMController `动画管理器` 可连续播放动画片段
+ SAMConfig `配置参数`
+ SAMBase : MonoBehaviour `动画片段基类` 包含 { SAMAlpha, SAMSize, SAMRotate ...}
  + SAMAction `事件`
  + SAMActive `活动`
  + SAMAlpha `透明度`
  + SAMAnimation `动画`
  + SAMBillboard `广告牌`
  + SAMCanvasGroup `CanvasGroup`
  + SAMGraphic `图形`
  + SAMRotate `旋转`
  + SAMRoute `路径`
  + SAMScale `大小`
  + SAMShake `抖动`
  + SAMSize `UI大小`
  + SAMTransform `位置, 方向，大小`

##资源下载模块 Renewable
+ RenewableResource `资源下载`
+ RenewableResourceUpdate `资源更新管理`
+ RenewableAssetBundle  `AssetBundle异步加载`
+ RenewableFile `文件管理`
+ Renewablepool `对象池`
+ RenewableUtils `扩展调用`
+ RenewableBase : MonoBehaviour
  + RenewableAsset `AssetBundle`
  + RenewableAudio `音频`
  + Renewablelmage `图片`
  + RenewableText  `文本`
+ Component : MonoBehaviour
  + RenewableComponent `关联组件`
  + RenewableImageComponent `图片`
  + RenewablePrefabComponent `预制体`
  + RenewableTextComponent `文本`

##扩展方法 Extension
- ExtensionAnimation `动画`
- ExtensionEmoji `图文混排`
- ExtensionGraphic `图形`
- ExtensionlEnumerable `迭代器`
- ExtensionLitson `Json`
- ExtensionNumber `数字`
- ExtensionString `字符串`
- ExtensionTransform `Transform`
- ExtensionUl `UI`

联系邮箱 ：202689420@qq.com








