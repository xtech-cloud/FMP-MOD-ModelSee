
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.88.0.  DO NOT EDIT!
//*************************************************************************************

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.ModelSee.LIB.Bridge;
using XTC.FMP.MOD.ModelSee.LIB.MVCS;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity
{
    /// <summary>
    /// 虚拟视图基类，用于处理消息订阅
    /// </summary>
    public class DummyViewBase : LibMVCS.View
    {
        public const string NAME = "XTC.FMP.MOD.ModelSee.LIB.Unity.DummyView";
        public MyRuntime runtime { get; set; }

        protected DummyModel model_ { get; set; }

        public DummyViewBase(string _uid) : base(_uid)
        {
        }

        protected override void preSetup()
        {
            model_ = findModel(DummyModelBase.NAME) as DummyModel;
        }

        protected override void setup()
        {
            addSubscriber(MySubjectBase.Create, handleCreate);
            addSubscriber(MySubjectBase.Open, handleOpen);
            addSubscriber(MySubjectBase.Show, handleShow);
            addSubscriber(MySubjectBase.Hide, handleHide);
            addSubscriber(MySubjectBase.Close, handleClose);
            addSubscriber(MySubjectBase.Delete, handleDelete);
            addSubscriber("/Bootloader/Step/Execute", handleBootloaderStepExecute);
        }

        private void handleCreate(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle create instance of {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            string style = "";
            string uiRoot = "";
            string uiSlot = "";
            string worldRoot = "";
            string worldSlot = "";
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                style = (string)data["style"];
                object objUiRoot;
                if (data.TryGetValue("uiRoot", out objUiRoot))
                {
                    uiRoot = objUiRoot as string;
                }
                object objUiSlot;
                if (data.TryGetValue("uiSlot", out objUiSlot))
                {
                    uiSlot = objUiSlot as string;
                }
                object objWorldRoot;
                if (data.TryGetValue("worldRoot", out objWorldRoot))
                {
                    worldRoot = objWorldRoot as string;
                }
                object objWorldSlot;
                if (data.TryGetValue("worldSlot", out objWorldSlot))
                {
                    worldSlot = objUiSlot as string;
                }
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
            }
            runtime.CreateInstanceAsync(uid, style, uiRoot, uiSlot, worldRoot, worldSlot, (_instance)=>
            {
            });
        }

        private void handleOpen(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle open instance of {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            string source = "";
            string uri = "";
            float delay = 0f;
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                delay = (float)data["delay"];
                object objSource;
                if (data.TryGetValue("source", out objSource))
                {
                    source = objSource as string;
                }
                object objUri;
                if (data.TryGetValue("uri", out objUri))
                {
                    uri = objUri as string;
                }
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
            }
            runtime.OpenInstanceAsync(uid, source, uri, delay);
        }

        private void handleShow(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle show instance of {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            float delay = 0f;
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                delay = (float)data["delay"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
            }
            runtime.ShowInstanceAsync(uid, delay);
        }

        private void handleHide(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle hide instance of {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            float delay = 0f;
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                delay = (float)data["delay"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
            }
            runtime.HideInstanceAsync(uid, delay);
        }

        private void handleClose(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle close instance of {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            float delay = 0f;
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
                delay = (float)data["delay"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
            }
            runtime.CloseInstanceAsync(uid, delay);
        }

        private void handleDelete(LibMVCS.Model.Status _status, object _data)
        {
            getLogger().Debug("handle delete instance of {0} with data: {1}", MyEntryBase.ModuleName, JsonConvert.SerializeObject(_data));
            string uid = "";
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = (string)data["uid"];
            }
            catch (Exception ex)
            {
                getLogger().Exception(ex);
            }
            runtime.DeleteInstanceAsync(uid);
        }

        private void handleBootloaderStepExecute(LibMVCS.Model.Status _status, object _data)
        {
            string module = _data as string;
            if (!MyEntryBase.ModuleName.Equals(module))
                return;

            model_.Publish("/Bootloader/Step/Finish", MyEntryBase.ModuleName);

            /*
            var signal = new LibMVCS.Signal(model_);
            signal.Connect((_status, _data) =>
            {
                getLogger().Info($"receive signal: {_data}");
            });
            signal.Emit("test");
            */
        }
    }
}

