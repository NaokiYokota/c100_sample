# Unity アニメーション本 
コミックマーケット100で配布を予定して同人誌の補足ページになります。
https://albatrus.com/entry/2022/07/28/190000

## 波の表現
次のコンポーネントをGameObjectに設定します。

 * SplitMeshComponent
 * UiWaveComponent

### 参考見た目
![wave_sample](https://user-images.githubusercontent.com/319724/183844139-9a5c5c23-cbaf-4cf1-8c6e-fe69ec8dd4a1.gif)


## 特殊フェード
次のコンポーネントをGameObjectに設定します。

 * DissolveImage

ShaderにDissolveImageを設定し、DissolveTextureにフェード用の画像を設定します。
Timeを調整して表示・非表示を行います。

![image](https://user-images.githubusercontent.com/319724/183838828-486f0839-161d-4388-a050-d1288a154693.png)

### 参考見た目
![dissole_sample](https://user-images.githubusercontent.com/319724/183844121-9a2cf262-6997-4068-91f4-55b872905a34.gif)
