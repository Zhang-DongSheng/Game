# Game
  Unity 工具类项目

##UI模块
+ UImanager `UI管理器`
+ UIConfig `配置参数`
+ UICtrlBase `控制基类`
+ UIBase `UI基类`

##事件通知模块
- EventManager `派发器`
- EventMessageArgs `事件参数`
- EventKey `索引`
```javascript
private void Send() {
    EventMessageArgs args = new EventMessageArgs();
    args.AddOrReplaceMessage("a", 1);
    args.AddOrReplaceMessage("b", "2");
    EventManager.PostEvent(EventKey.Login, args);
}
```

##动画模块 SAM (Sample Animation)
+ SAMController `动画管理器` 可自定义添加动画片段控制播放
+ SAMConfig `配置参数`
+ SAMBase `动画片段基类` 包含 { SAMAlpha, SAMSize, SAMRotate ...}
  + SAMAction `事件`
  + SAMActive `活动`
  + SAMAlpha `透明度`
  + SAMAnimation `动画`
  + SAMBillboard `广告牌`
  + SAMCanvas `CanvasGroup`
  + SAMGraphic `图形`
  + SAMRotate `旋转`
  + SAMRoute `路径`
  + SAMShake `抖动`
  + SAMSize `大小`
  + SAMTransform `位置, 方向，大小`

##资源下载模块 Renewable
+ RenewableResource `资源下载`
+ RenewableResourceUpdate `资源更新管理`
+ RenewableAssetBundle  `AssetBundle异步加载`
+ RenewableFile `文件管理`
+ Renewablepool `对象池`
+ RenewableUtils `扩展调用`
+ RenewableBase
  + RenewableAsset `AssetBundle`
  + RenewableAudio `音频`
  + Renewablelmage  `图片`
  + RenewableText  `文本`
+ Component : MonoBehaviour
  + RenewablelmageComponent `图片组件`
  + RenewablePrefabComponent `预制体组件`
  + RenewableTextComponent `文本组件`

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








