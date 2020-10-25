using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Flex.Smoothlake.FlexLib;
using FRStack;


/// <summary>
/// FRStack version 3 Custom Functions allows you to extended the FRStack Menu, Hotkeys and Rest API with your own .NET DLL.
/// 
/// MIT License
/// Copyright (c) 2020 MKCM Software, LLC
/// 
/// Author: Mark Hanson
/// 
/// </summary>
namespace FRStackFunctions
{
    public class SampleFunctions : RadioFunctionsBase
    {

        /// <summary>
        /// Polling call from FRStack's polling loop
        /// 
        /// This method is called every 500ms
        /// Your logic should be very fast here. 
        /// If you have longer running logic you should spin up your own Task.
        /// </summary>
        public override void Poll()
        {

        }

        /// <summary>
        /// Initialize the functions.
        /// 
        /// 
        /// </summary>
        /// <param name="radioIn"></param>
        /// <param name="restcall"></param>
        public override void Init(Radio radioIn, Func<string, string, string, Task<object>> restcall)
        {
            // 1. Call base class init
            base.Init(radioIn, restcall);

            // 2. If radio object provided then hook any desired events
            if (radio != null)
            {
                radio.PropertyChanged += Radio_PropertyChanged;
                radio.SliceAdded += Radio_SliceAdded;
                radio.SliceRemoved += Radio_SliceRemoved;
            }

            // 3. Create custom function item array
            _items = new RadioFunctionItem[]
            {
                new RadioFunctionItem()
                {
                        enabled = true,
                        hidden = false,
                        menu = "Say _Hello",
                        rfunct = (parms) => {  // inline method that returns Hello
                            string sret = "Hello";
                            return Task.FromResult((object)sret);
                        }
                },
                new RadioFunctionItem()
                {
                        enabled = true,
                        hidden = false,
                        menu = "_Disable toggle menu items",
                        rfunct = ExecuteSetEnableFunction
                },
                new RadioFunctionItem()
                {
                        enabled = true,
                        hidden = radio == null,
                        menu = "Toggle _MOX",
                        rfunct = (parms) => {  // inline method that toggles MOX
                            if (radio != null)
                                radio.Mox = radio.Mox ? false : true;
                            return Task.FromResult((object)string.Empty); 
                        }
                },
                new RadioFunctionItem()
                {
                        enabled = true,
                        hidden = radio == null,
                        menu = "Rest Radio Info",
                        rfunct = ExecuteUseRestApiFunction
                },
                new RadioFunctionItem()
                {
                        enabled = true,
                        hidden = false,
                        menu = "SPE Amp Window Toggle",
                        rfunct = (parms) => {  // inline method that calls Rest API to toggle SPE Window
                            return makeRestCall("FRStack", "SPEWINDOW", "2");
                        }
                }
            };
        }

        /// <summary>
        /// Clean up any events you placed against object model
        /// </summary>
        public override void Cleanup()
        {
            if (radio != null)
            {
                radio.PropertyChanged -= Radio_PropertyChanged;
                radio.SliceAdded -= Radio_SliceAdded;
                radio.SliceRemoved -= Radio_SliceRemoved;
            }
            base.Cleanup();
        }

        /// <summary>
        /// This samples shows how you can toggle enable for menu items
        /// </summary>
        /// <param name="fparams"></param>
        /// <returns></returns>
        private Task<object> ExecuteSetEnableFunction(object[] fparams)
        {
            for (int ii = 0; ii < Items.Length; ii++)
            {
                // skip this menu item and any hidden ones
                if (ii == 1 || Items[ii].hidden)
                    continue;

                Items[ii].enabled = Items[ii].enabled ? false : true;
            }
            return Task.FromResult((object)string.Empty);
        }


        /// <summary>
        /// Sample that displays Rest API's Radio INFO
        /// 
        /// So you like the Rest API and you want to chain several calls together under its own Rest API
        ///
        /// uses async modifier to wait for results then decide what data type to return
        /// 
        /// </summary>
        /// <param name="fparams"></param>
        /// <returns></returns>
        private async Task<object> ExecuteUseRestApiFunction(object[] fparams)
        {
            object oret = await makeRestCall("RADIO", "INFO", null);

            Debug.Assert(fparams == null || fparams.Length == 0, "Custom Function requires Rest, Hotkey or Menu as first parameter!");

            // if called by REST then don't render as string
            if (string.Compare((fparams as string[])?[0], "Rest", true) == 0)
                return oret;

            // menu or hotkey so going to message box so convert to JSON encoded string
            return Newtonsoft.Json.JsonConvert.SerializeObject(oret);
        }


        /// <summary>
        /// Show how to track radio object property changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Radio_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BoundClientID") // prop event when non GUI client is bound to new client
            {

            }
        }

        /// <summary>
        /// New slice event
        /// </summary>
        /// <param name="slc"></param>
        private void Radio_SliceAdded(Slice slc)
        {

        }
        /// <summary>
        /// Removed slice event
        /// </summary>
        /// <param name="slc"></param>
        private void Radio_SliceRemoved(Slice slc)
        {

        }


    }
}
