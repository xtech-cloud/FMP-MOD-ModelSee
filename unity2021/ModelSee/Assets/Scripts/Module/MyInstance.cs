

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.ModelSee.LIB.Proto;
using XTC.FMP.MOD.ModelSee.LIB.MVCS;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;
using HedgehogTeam.EasyTouch;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity
{
    /// <summary>
    /// 实例类
    /// </summary>
    public class MyInstance : MyInstanceBase
    {
        private class UiReference
        {
            public RawImage renderer;
            public RectTransform sidemenu;
            public Toggle menuFocus;
            public Toggle menuFeatures;
            public Toggle featureTempalte;
            public RectTransform featureContainer;
            public RectTransform featureInfo;
            public RectTransform featureMask;
            public Toggle firstFeature;
        }

        private class WorldReference
        {
            public Camera renderCamera;
            public Transform pivot;
        }

        private UiReference uiReference_ = new UiReference();
        private WorldReference worldReference_ = new WorldReference();
        private ManifestSchema manifestSchema_ = null;
        private Toggle currentFeature_;
        private AssetBundle loadedAssetBundle_ = null;
        private GameObject agentClone_ = null;
        private Coroutine loadAgentCoroutine_ = null;
        private int gridSpaceCellIndex_ = -1;
        private bool allowGestureOperation_ = false;

        public MyInstance(string _uid, string _style, MyConfig _config, MyCatalog _catalog, LibMVCS.Logger _logger, Dictionary<string, LibMVCS.Any> _settings, MyEntryBase _entry, MonoBehaviour _mono, GameObject _rootAttachments)
            : base(_uid, _style, _config, _catalog, _logger, _settings, _entry, _mono, _rootAttachments)
        {
        }

        /// <summary>
        /// 当被创建时
        /// </summary>
        /// <remarks>
        /// 可用于加载主题目录的数据
        /// </remarks>
        public void HandleCreated()
        {

            uiReference_.renderer = rootUI.transform.Find("renderer").GetComponent<RawImage>();
            uiReference_.featureContainer = rootUI.transform.Find("featureS").GetComponent<RectTransform>();
            uiReference_.featureInfo = rootUI.transform.Find("featureInfo").GetComponent<RectTransform>();
            uiReference_.featureMask = rootUI.transform.Find("featureMask").GetComponent<RectTransform>();
            uiReference_.featureTempalte = rootUI.transform.Find("featureS/featureTemplate").GetComponent<Toggle>();
            uiReference_.sidemenu = rootUI.transform.Find("sidemenu").GetComponent<RectTransform>();
            uiReference_.menuFocus = rootUI.transform.Find("sidemenu/focus").GetComponent<Toggle>();
            uiReference_.menuFeatures = rootUI.transform.Find("sidemenu/features").GetComponent<Toggle>();
            worldReference_.renderCamera = rootWorld.transform.Find("renderCamera").GetComponent<Camera>();
            worldReference_.pivot = rootWorld.transform.Find("pivot");

            uiReference_.featureTempalte.gameObject.SetActive(false);
            uiReference_.featureContainer.gameObject.SetActive(false);
            uiReference_.featureInfo.gameObject.SetActive(false);
            uiReference_.featureMask.gameObject.SetActive(false);

            // 创建网格空间
            {
                var anchor = new Vector3(style_.gridSpace.anchorX, style_.gridSpace.anchorY, style_.gridSpace.anchorZ);
                GridSpace.singleton.CreateZone(style_.name, GridSpace.Mode.Y, anchor, style_.gridSpace.cellSize);

                gridSpaceCellIndex_ = GridSpace.singleton.NewCell(style_.name);
                Vector3 gridSpaceCellCenter = GridSpace.singleton.GetCellCenter(style_.name, gridSpaceCellIndex_);
                // 将世界根节点移到网格空间单元的中心点
                rootWorld.transform.position = gridSpaceCellCenter;
            }

            // 边栏菜单
            {
                var glg = uiReference_.sidemenu.GetComponent<GridLayoutGroup>();
                glg.cellSize = new Vector2(style_.sideMenu.itemWidth, style_.sideMenu.itemHeight);
                glg.spacing = new Vector2(0, style_.sideMenu.itemSpace);
                loadTextureFromTheme(style_.sideMenu.focusTab.uncheckedImage, (_texture) =>
                {
                    uiReference_.menuFocus.transform.Find("Background").GetComponent<RawImage>().texture = _texture;
                }, () => { });
                loadTextureFromTheme(style_.sideMenu.focusTab.checkedImage, (_texture) =>
                {
                    uiReference_.menuFocus.transform.Find("Checkmark").GetComponent<RawImage>().texture = _texture;
                }, () => { });
                loadTextureFromTheme(style_.sideMenu.featureTab.uncheckedImage, (_texture) =>
                {
                    uiReference_.menuFeatures.transform.Find("Background").GetComponent<RawImage>().texture = _texture;
                }, () => { });
                loadTextureFromTheme(style_.sideMenu.featureTab.checkedImage, (_texture) =>
                {
                    uiReference_.menuFeatures.transform.Find("Checkmark").GetComponent<RawImage>().texture = _texture;
                }, () => { });
            }

            // 特征面板
            {
                var rt = uiReference_.featureTempalte.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(style_.featurePanel.featureTab.width, style_.featurePanel.featureTab.height);
                var lg = uiReference_.featureContainer.GetComponent<VerticalLayoutGroup>();
                lg.spacing = style_.featurePanel.tabSpace;
                var tabLabel = uiReference_.featureTempalte.transform.Find("Label").GetComponent<Text>();
                tabLabel.fontSize = style_.featurePanel.featureTab.fontSize;
                Color fontColor;
                if (!ColorUtility.TryParseHtmlString(style_.featurePanel.featureTab.fontColor, out fontColor))
                    fontColor = Color.black;
                tabLabel.color = fontColor;
                loadTextureFromTheme(style_.featurePanel.featureTab.uncheckedImage, (_texture) =>
                {
                    uiReference_.featureTempalte.transform.Find("Background").GetComponent<RawImage>().texture = _texture;
                }, () => { });
                loadTextureFromTheme(style_.featurePanel.featureTab.checkedImage, (_texture) =>
                {
                    uiReference_.featureTempalte.transform.Find("Checkmark").GetComponent<RawImage>().texture = _texture;
                }, () => { });
            }

            // 特征遮罩
            {
                if (style_.featurePanel.featureMask.active)
                {
                    loadTextureFromTheme(style_.featurePanel.featureMask.image, (_texture) =>
                    {
                        uiReference_.featureMask.GetComponent<RawImage>().texture = _texture;
                    }, () => { });
                }
            }

            // 特征卡片
            {
                var image = uiReference_.featureInfo.GetComponent<Image>();
                image.rectTransform.sizeDelta = new Vector2(style_.featurePanel.featureCard.width, style_.featurePanel.featureCard.height);
                loadTextureFromTheme(style_.featurePanel.featureCard.image, (_texture) =>
                {
                    var border = Vector4.one * style_.featurePanel.featureCard.border;
                    var sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, border);
                    image.sprite = sprite;
                }, () => { });

                var nameText = uiReference_.featureInfo.Find("name").GetComponent<Text>();
                nameText.font = settings_["font.main"].AsObject() as Font;
                nameText.fontSize = style_.featurePanel.featureCard.nameFontSize;
                Color nameColor;
                if (!ColorUtility.TryParseHtmlString(style_.featurePanel.featureCard.nameFontColor, out nameColor))
                    nameColor = Color.white;
                nameText.color = nameColor;

                var descriptionText = uiReference_.featureInfo.Find("description").GetComponent<Text>();
                descriptionText.font = settings_["font.main"].AsObject() as Font;
                descriptionText.fontSize = style_.featurePanel.featureCard.descriptionFontSize;
                Color descriptionColor;
                if (!ColorUtility.TryParseHtmlString(style_.featurePanel.featureCard.descriptionFontColor, out descriptionColor))
                    descriptionColor = Color.white;
                descriptionText.color = descriptionColor;
            }

            // 手势
            {
                // 水平滑动
                var swipeH = uiReference_.renderer.gameObject.AddComponent<QuickSwipe>();
                swipeH.swipeDirection = QuickSwipe.SwipeDirection.Horizontal;
                swipeH.enablePickOverUI = true;
                swipeH.onSwipeAction = new QuickSwipe.OnSwipeAction();
                swipeH.onSwipeAction.AddListener((_gesture) =>
                {
                    if (!allowGestureOperation_)
                        return;
                    if (null == _gesture.pickedUIElement)
                        return;
                    if (uiReference_.renderer.gameObject != _gesture.pickedUIElement)
                        return;
                    worldReference_.renderCamera.transform.RotateAround(worldReference_.pivot.transform.position, Vector3.up, _gesture.swipeVector.x * Time.deltaTime * style_.gesture.swipH.speed);
                });

                // 垂直滑动
                var swipeV = uiReference_.renderer.gameObject.AddComponent<QuickSwipe>();
                swipeV.swipeDirection = QuickSwipe.SwipeDirection.Vertical;
                swipeV.enablePickOverUI = true;
                swipeV.onSwipeAction = new QuickSwipe.OnSwipeAction();
                swipeV.onSwipeAction.AddListener((_gesture) =>
                {
                    if (!allowGestureOperation_)
                        return;
                    if (null == _gesture.pickedUIElement)
                        return;
                    if (uiReference_.renderer.gameObject != _gesture.pickedUIElement)
                        return;
                    worldReference_.renderCamera.transform.RotateAround(worldReference_.pivot.transform.position, worldReference_.renderCamera.transform.right, _gesture.swipeVector.y * Time.deltaTime * style_.gesture.swipH.speed);
                });

                // 捏合
                var pinch = uiReference_.renderer.gameObject.AddComponent<QuickPinch>();
                pinch.onPinchAction = new QuickPinch.OnPinchAction();
                pinch.enablePickOverUI = true;
                pinch.onPinchAction.AddListener((_gesture) =>
                {
                    if (!allowGestureOperation_)
                        return;
                    if (null == _gesture.pickedUIElement)
                        return;
                    if (uiReference_.renderer.gameObject != _gesture.pickedUIElement)
                        return;

                    // 距离约束
                    {
                        var renderCameraFocusPosition = new Vector3(manifestSchema_.focus.renderCamera.position.x,
                            manifestSchema_.focus.renderCamera.position.y,
                            manifestSchema_.focus.renderCamera.position.z);

                        var pivotFocusPosition = new Vector3(manifestSchema_.focus.pivot.position.x,
                            manifestSchema_.focus.pivot.position.y,
                            manifestSchema_.focus.pivot.position.z);

                        Vector3 renderCamerePosition = worldReference_.renderCamera.transform.localPosition + worldReference_.renderCamera.transform.forward * Time.deltaTime * _gesture.deltaPinch * style_.gesture.pinch.speed;
                        float scale = Vector3.Distance(renderCameraFocusPosition, pivotFocusPosition) / Vector3.Distance(renderCamerePosition, pivotFocusPosition);
                        if (scale < style_.gesture.pinch.minScale || scale > style_.gesture.pinch.maxScale)
                            return;
                    }

                    worldReference_.renderCamera.transform.Translate(Vector3.forward * _gesture.deltaPinch * Time.deltaTime * style_.gesture.pinch.speed, Space.Self);
                    worldReference_.renderCamera.transform.LookAt(worldReference_.pivot);
                });
                allowGestureOperation_ = true;
            }


            //创建渲染纹理
            int renderTextureWidth = (int)uiReference_.renderer.rectTransform.rect.width;
            int renderTextureHeight = (int)uiReference_.renderer.rectTransform.rect.height;
            var renderTexture = new RenderTexture(renderTextureWidth, renderTextureHeight, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            uiReference_.renderer.texture = renderTexture;
            worldReference_.renderCamera.targetTexture = renderTexture;

            rootUI.transform.Find("DebugUtility").gameObject.SetActive(style_.debug.active);
            rootWorld.transform.Find("pivot/mesh").gameObject.SetActive(style_.debug.active);
            if (style_.debug.active)
            {
                var debugUtility = rootUI.AddComponent<DebugUtility>();
                debugUtility.renderCamera = worldReference_.renderCamera;
                debugUtility.pivot = rootWorld.transform.Find("pivot");
                debugUtility.pivot.localScale = Vector3.one * style_.debug.pivotScale;
                debugUtility.moveSpeed = style_.debug.moveSpeed;
                debugUtility.debugPosX = rootUI.transform.Find("DebugUtility/Panel/pos_x").GetComponent<Text>();
                debugUtility.debugPosY = rootUI.transform.Find("DebugUtility/Panel/pos_y").GetComponent<Text>();
                debugUtility.debugPosZ = rootUI.transform.Find("DebugUtility/Panel/pos_z").GetComponent<Text>();
                debugUtility.debugRotX = rootUI.transform.Find("DebugUtility/Panel/rot_x").GetComponent<Text>();
                debugUtility.debugRotY = rootUI.transform.Find("DebugUtility/Panel/rot_y").GetComponent<Text>();
                debugUtility.debugRotZ = rootUI.transform.Find("DebugUtility/Panel/rot_z").GetComponent<Text>();
                debugUtility.debugPivotX = rootUI.transform.Find("DebugUtility/Panel/pivot_x").GetComponent<Text>();
                debugUtility.debugPivotY = rootUI.transform.Find("DebugUtility/Panel/pivot_y").GetComponent<Text>();
                debugUtility.debugPivotZ = rootUI.transform.Find("DebugUtility/Panel/pivot_z").GetComponent<Text>();
            }


            // 绑定事件
            uiReference_.menuFocus.onValueChanged.AddListener((_toggled) =>
            {
                allowGestureOperation_ = _toggled;
                if (!_toggled)
                    return;
                applyFocus();
            });
            uiReference_.menuFeatures.onValueChanged.AddListener((_toggled) =>
            {
                uiReference_.featureContainer.gameObject.SetActive(_toggled);
                uiReference_.featureInfo.gameObject.SetActive(_toggled);
                uiReference_.featureMask.gameObject.SetActive(_toggled && style_.featurePanel.featureMask.active);
                if (!_toggled)
                {
                    uiReference_.featureTempalte.isOn = true;
                    currentFeature_.isOn = false;
                    return;
                }
                uiReference_.firstFeature.isOn = true;
            });
        }

        /// <summary>
        /// 当被删除时
        /// </summary>
        public void HandleDeleted()
        {
            GridSpace.singleton.DeleteCell(style_.name, gridSpaceCellIndex_);
        }

        /// <summary>
        /// 当被打开时
        /// </summary>
        /// <remarks>
        /// 可用于加载内容目录的数据
        /// </remarks>
        public void HandleOpened(string _source, string _uri)
        {
            rootUI.gameObject.SetActive(true);
            rootWorld.gameObject.SetActive(true);
            handleOpened(_source, _uri);
        }

        /// <summary>
        /// 当被关闭时
        /// </summary>
        public void HandleClosed()
        {
            rootUI.gameObject.SetActive(false);
            rootWorld.gameObject.SetActive(false);
            handleClosed();
        }


        private void handleOpened(string _source, string _uri)
        {
            string uri = "";
            if (_source == "assloud://")
            {
                uri = string.Format("{0}/{1}", settings_["path.assets"].AsString(), _uri);
            }

            string dir = Path.GetDirectoryName(uri);
            string filename = Path.GetFileNameWithoutExtension(uri);
            string extension = Path.GetExtension(uri);
            uri = string.Format("{0}/{1}{2}", dir, filename + "@" + settings_["platform"].AsString(), extension);
            uri = uri.Replace("\\", "/");
            logger_.Debug("real uri is {0}", uri);

            if (uri.EndsWith("_"))
            {
                parseManifestFromFile(uri);
                loadAgentCoroutine_ = mono_.StartCoroutine(loadUABFromFile(uri));
            }
            applyFocus();
            createFeatures();
        }

        private void handleClosed()
        {
            if (null != loadAgentCoroutine_)
            {
                mono_.StopCoroutine(loadAgentCoroutine_);
                loadAgentCoroutine_ = null;
            }
            if (null != agentClone_)
            {
                GameObject.Destroy(agentClone_);
                agentClone_ = null;
            }
            if (null != loadedAssetBundle_)
            {
                loadedAssetBundle_.Unload(true);
                loadedAssetBundle_ = null;
            }
        }

        private void parseManifestFromFile(string _uir)
        {
            string file = string.Format("{0}/manifest.json", _uir);
            if (!File.Exists(file))
            {
                logger_.Error("{0} not exists", file);
                return;
            }

            manifestSchema_ = null;
            try
            {
                var bytes = File.ReadAllBytes(file);
                var json = System.Text.Encoding.UTF8.GetString(bytes);
                manifestSchema_ = JsonConvert.DeserializeObject<ManifestSchema>(json);
            }
            catch (System.Exception ex)
            {
                logger_.Exception(ex);
            }

            if (manifestSchema_ == null)
            {
                logger_.Error("parse maanifest.json failed!");
            }
        }

        private void applyFocus()
        {
            var renderCameraPosition = new Vector3(manifestSchema_.focus.renderCamera.position.x,
                manifestSchema_.focus.renderCamera.position.y,
                manifestSchema_.focus.renderCamera.position.z);

            var pivotPosition = new Vector3(manifestSchema_.focus.pivot.position.x,
                manifestSchema_.focus.pivot.position.y,
                manifestSchema_.focus.pivot.position.z);

            worldReference_.renderCamera.transform.localPosition = renderCameraPosition;
            worldReference_.pivot.transform.localPosition = pivotPosition;

            worldReference_.renderCamera.transform.LookAt(worldReference_.pivot);
        }

        private void createFeatures()
        {
            foreach (var feature in manifestSchema_.featureS)
            {
                var clone = GameObject.Instantiate(uiReference_.featureTempalte.gameObject, uiReference_.featureTempalte.transform.parent);
                clone.transform.Find("Label").GetComponent<Text>().text = feature.name;
                clone.gameObject.SetActive(true);
                var toggle = clone.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener((_toggled) =>
                {
                    if (!_toggled)
                        return;
                    currentFeature_ = toggle;
                    switchFeature(feature);
                });
                if (null == uiReference_.firstFeature)
                    uiReference_.firstFeature = toggle;
            }
        }

        private void switchFeature(ManifestSchema.Feature _feature)
        {
            var renderCameraPosition = new Vector3(_feature.renderCamera.position.x,
                _feature.renderCamera.position.y,
                _feature.renderCamera.position.z);

            var pivotPosition = new Vector3(_feature.pivot.position.x,
                _feature.pivot.position.y,
                _feature.pivot.position.z);

            worldReference_.renderCamera.transform.localPosition = renderCameraPosition;
            worldReference_.pivot.transform.localPosition = pivotPosition;

            worldReference_.renderCamera.transform.LookAt(worldReference_.pivot);

            uiReference_.featureInfo.Find("name").GetComponent<Text>().text = _feature.name;
            uiReference_.featureInfo.Find("description").GetComponent<Text>().text = _feature.description;
        }

        private IEnumerator loadUABFromFile(string _uri)
        {
            string file = string.Format("{0}/{1}.uab", _uri, settings_["platform"].AsString());
            if (!File.Exists(file))
            {
                logger_.Error("{0} not exists", file);
                yield break;
            }

            //logger_.Warning("11111111");
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(file);
            //var bundleLoadRequest = UnityWebRequestAssetBundle.GetAssetBundle(file);
            //logger_.Warning("222222");
            //yield return bundleLoadRequest.SendWebRequest();
            yield return bundleLoadRequest;
            //logger_.Warning("3333333");

            loadedAssetBundle_ = bundleLoadRequest.assetBundle;
            //loadedAssetBundle_ = DownloadHandlerAssetBundle.GetContent(bundleLoadRequest);
            //logger_.Warning("4444444");
            if (null == loadedAssetBundle_)
            {
                logger_.Error("load agent failed!");
                yield break;
            }

            var assetLoadRequest = loadedAssetBundle_.LoadAssetAsync<GameObject>("agent");
            yield return assetLoadRequest;

            GameObject agent = assetLoadRequest.asset as GameObject;
            agentClone_ = GameObject.Instantiate(agent);
            agentClone_.transform.SetParent(rootWorld.transform);
            agentClone_.transform.localPosition = Vector3.zero;
        }

    }
}
