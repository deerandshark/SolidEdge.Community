﻿using SolidEdge.Community;
using SolidEdge.Community.AddIn;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TestAddIn
{
    [ComVisible(true)]
    [Guid("00000000-0000-0000-0000-C7823CD102E4")]
    [ProgId("SolidEdge.Community.TestAddIn.MyAddIn")]
    public class MyAddIn : SolidEdge.Community.AddIn.SolidEdgeAddIn
    {
        public override void OnConnection(SolidEdgeFramework.Application application, SolidEdgeFramework.SeConnectMode ConnectMode, SolidEdgeFramework.AddIn AddInInstance)
        {
            base.OnConnection(application, ConnectMode, AddInInstance);

            // If you makes changes to your ribbon, be sure to increment the GuiVersion. That makes bFirstTime = true
            // next time Solid Edge is started. bFirstTime is used to setup the ribbon so if you make a change but don't
            // see the changes, that could be why
            AddInEx.GuiVersion = 1;

            // Put your custom OnConnection code here.
            var applicationEvents = (SolidEdgeFramework.ISEApplicationEvents_Event)application.ApplicationEvents;
            applicationEvents.AfterWindowActivate += applicationEvents_AfterWindowActivate;
        }

        void applicationEvents_AfterWindowActivate(object theWindow)
        {
            var window = (SolidEdgeFramework.Window)theWindow;
            this.OverlayController.Add<MyViewOverlay>(window.View);
        }

        public override void OnConnectToEnvironment(SolidEdgeFramework.Environment environment, bool firstTime)
        {
            base.OnConnectToEnvironment(environment, firstTime);
        }

        public override void OnDisconnection(SolidEdgeFramework.SeDisconnectMode DisconnectMode)
        {
            base.OnDisconnection(DisconnectMode);
        }

        public override void OnCreateRibbon(RibbonController controller, Guid environmentCategory, bool firstTime)
        {
            controller.Add<MyRibbon>(environmentCategory, firstTime);
        }

        public override void OnCreateEdgeBarPage(EdgeBarController controller, SolidEdgeFramework.SolidEdgeDocument document)
        {
            controller.Add<MyEdgeBarControl>(document, 1);
        }

        #region Registration functions

        [ComRegisterFunction]
        public static void OnRegister(Type t)
        {
            string title = "SolidEdge.Community.TestAddIn";
            string summary = "Solid Edge Addin in .NET 4.0.";

            Guid[] environments = {
                                        SolidEdge.CATID.SEApplicationGuid,
                                        SolidEdge.CATID.SEAllDocumentEnvrionmentsGuid
                                    };

            try
            {
                SolidEdge.Community.AddIn.SolidEdgeAddIn.Register(t, title, summary, environments);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
        }

        [ComUnregisterFunction]
        public static void OnUnregister(Type t)
        {
            SolidEdge.Community.AddIn.SolidEdgeAddIn.Unregister(t);
        }

        #endregion
    }
}