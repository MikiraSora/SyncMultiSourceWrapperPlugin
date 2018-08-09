## SyncMultiSourceWrapperPlugin

### 简介
本插件可以将现有几个source一起使用，达到多平台多弹幕同时交流功能，玩家也能通过固定格式的消息发送消息到指定的source上.

### 用法
1. 请先设置好你要用的Source的配置
2. 在config.ini的`[SyncMultiSourceWrapperPlugin.Setting]`中，将`SourceList`填写你需要使用的Source的名字，比如"`bilibili,twtich`",目前仅支持:
* B站直播 bilibili
* Twitch直播 twitch
多个名字请用英文逗号分开
* 在config.ini的`[Sync.DefaultConfiguration]`中，设置`Source`为"`SyncMultiSourceWrapper`"
* 保存并打开Sync,按通常方法输入"`start`"食用

### 相关命令
* 在游戏内，各个Source消息都会发往osu!irc通常消息的用户名称并不会标识Source来源，但你可以通过将'MarkSource'设置为"`true`"来给用户添加来源标识,如图
![](https://puu.sh/Bbqrx/ddfc418f7e.png)

* 本插件也提供格式来指定将玩家消息发往哪一个Source，格式如下:
>
> [(index)或(part_source_name)]>:message<br><br>
> 比如:<br>
> 0:zzzz //将"zzzz"发到第一个source<br><br>
> 0:yyyy //将"yyyy"发到第二个source<br><br>
> bii:zztt //将"zztt"发到source名字以bii开头的source

如果不按照此格式，默认将此消息原文不动的发到所有source.