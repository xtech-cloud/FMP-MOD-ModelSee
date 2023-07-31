using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using LibMVCS = XTC.FMP.LIB.MVCS;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity.AssetBundleUtility
{
    /// <summary>
    /// 缓存策略
    /// </summary>
    public abstract class CacheStrategy
    {
        protected MonoBehaviour mono_ { get; private set; }
        protected LibMVCS.Logger logger_ { get; private set; }
        protected List<AssetBundleLoadTask> taskS_ = new List<AssetBundleLoadTask>();
        protected List<LoadedAssetBundle> loadedAssetBundleS_ = new List<LoadedAssetBundle>();
        public abstract void Load(string _uri, UnityAction<AssetBundle> _onFinish);

        public CacheStrategy(MonoBehaviour _mono, LibMVCS.Logger _logger)
        {
            mono_ = _mono;
            logger_ = _logger;
        }


        protected void load(string _uri, UnityAction<AssetBundle> _onFinish)
        {
            // 缓存中存在，直接使用
            var loadedAssetBundle = loadedAssetBundleS_.Find(x => x.uri == _uri);
            if (null != loadedAssetBundle)
            {
                _onFinish(loadedAssetBundle.assetBundle);
                return;
            }

            AssetBundleLoadTask task = taskS_.Find((_item) => { return _item.uri == _uri; });
            // 缓存中不存在，但存在加载任务，添加事件回调
            if (null != task)
            {
                task.onFinish.AddListener(_onFinish);
                return;
            }
            // 缓存中不存在，且加载任务不存在，新建任务开始加载
            task = new AssetBundleLoadTask(_uri);
            task.onFinish.AddListener(_onFinish);
            taskS_.Add(task);
            mono_.StartCoroutine(loadAssetBundle(task));
        }

        private IEnumerator loadAssetBundle(AssetBundleLoadTask _task)
        {
            if (!File.Exists(_task.uri))
            {
                logger_.Error("{0} not exists", _task.uri);
                yield break;
            }

            var bundleLoadRequest = UnityWebRequestAssetBundle.GetAssetBundle(_task.uri);
            yield return bundleLoadRequest.SendWebRequest();
            var assetBundle = DownloadHandlerAssetBundle.GetContent(bundleLoadRequest);
            if (null == assetBundle)
            {
                logger_.Error("load AssetBundle failed!");
                yield break;
            }
            LoadedAssetBundle loadedAssetBundle = new LoadedAssetBundle();
            loadedAssetBundle.uri = _task.uri;
            loadedAssetBundle.assetBundle = assetBundle;
            loadedAssetBundleS_.Add(loadedAssetBundle);
            _task.onFinish.Invoke(assetBundle);
            taskS_.Remove(_task);
        }
    }//class
}//namespace
