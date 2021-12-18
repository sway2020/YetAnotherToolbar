﻿using System;
using ColossalFramework.UI;
using UnityEngine;
using System.Reflection;
using ColossalFramework.Plugins;
using System.Collections.Generic;

namespace YetAnotherToolbar
{
    public class YetAnotherToolbar : MonoBehaviour
    {
        public static YetAnotherToolbar instance;
        public static UITextureAtlas atlas = LoadResources();
        public UIMainButton mainButton;
        private bool initialized = false;
        private UITabContainer tsContainer;
        public static bool isFindItEnabled = IsAssemblyEnabled("findit");
        public static bool isRICOEnabled = IsAssemblyEnabled("ploppablerico");
        public static bool isEditorMode = false;
        private Dictionary<UIPanel, UIScrollbar> dictVerticalScrollbars = new Dictionary<UIPanel, UIScrollbar>();
        public bool shownUpdateNoticeFlag = false;

        private bool hideMenuFlag = false;
        private Vector2 lastMenuPosition;

        private static UISlicedSprite thumbnailBar;
        private static UISlicedSprite tsBar;
        private static UIPanel infoPanel;

        private static UIComponent pauseOutline;
        private static Vector2 pauseOutlineOriginalSize;
        private static Vector2 originalScreenSize;

        public void Start()
        {
            try
            {
                if (mainButton == null)
                {
                    tsContainer = GameObject.Find("TSContainer").GetComponent<UITabContainer>();
                    thumbnailBar = UIView.Find<UISlicedSprite>("ThumbnailBar");
                    tsBar = UIView.Find<UISlicedSprite>("TSBar");
                    infoPanel = UIView.Find<UIPanel>("InfoPanel");
                    pauseOutline = GameObject.Find("PauseOutline").GetComponent<UIComponent>();
                    pauseOutlineOriginalSize = pauseOutline.size;

                    UIView view = UIView.GetAView();
                    Vector2 screenResolution = view.GetScreenResolution();
                    originalScreenSize = screenResolution;
                    //UIMultiStateButton advisorButton = view.FindUIComponent<UIMultiStateButton>("AdvisorButton");

                    // Set Advisor Button and filter panel visiblity
                    SetAdvisorButtonVisibility();
                    // SetFilterPanelsVisibility();
                    mainButton = (UIMainButton)view.AddUIComponent(typeof(UIMainButton));
                    mainButton.absolutePosition = new Vector3(Settings.mainButtonX * screenResolution.x / 1920f, Settings.mainButtonY * screenResolution.y / 1080f);// advisorButton.absolutePosition + new Vector3(advisorButton.width, 0);
                    mainButton.name = "YetAnotherToolbarMainButton";
                    mainButton.isInteractive = true;
                    mainButton.size = new Vector2(34, 34);
                    mainButton.isVisible = !Settings.hideMainButton;
                    mainButton.atlas = YetAnotherToolbar.atlas;

                    if (!Settings.expanded)
                    {
                        mainButton.normalFgSprite = "Expand";
                    }
                    else
                    {
                        mainButton.normalFgSprite = "Collapse";

                    }
                    mainButton.eventClicked += (c, p) =>
                    {

                        if (Settings.expanded)
                        {
                            Settings.expanded = false;
                            XMLUtils.SaveSettings();
                            Collapse();
                            mainButton.normalFgSprite = "Expand";
                        }
                        else
                        {
                            Settings.expanded = true;
                            XMLUtils.SaveSettings();
                            Expand();
                            mainButton.normalFgSprite = "Collapse";
                        }

                    };

                }
            }
            catch (Exception ex)
            {
                Debugging.Message("Start() - " + ex.Message);
            }
        }

        public void Update()
        {
            if (!initialized)
            {
                initialized = true;
                if (!Settings.expanded)
                {
                    Collapse();
                }
                else
                {
                    Expand();
                }

                UpdateMainPanelBackground();
                UpdateThumbnailBarBackground();
                UpdateTSBarBackground();
                UpdateInfoPanelBackground();

                // show update notice
                if (!YetAnotherToolbar.instance.shownUpdateNoticeFlag)
                {
                    YetAnotherToolbar.instance.shownUpdateNoticeFlag = true;
                    // show update notice
                    if (!Settings.disableUpdateNotice && (ModInfo.updateNoticeDate > Settings.lastUpdateNotice))
                    {
                        UIUpdateNoticePopUp.ShowAt();
                        Settings.lastUpdateNotice = ModInfo.updateNoticeDate;
                        XMLUtils.SaveSettings();
                    }
                }
            }
        }

        private static UITextureAtlas LoadResources()
        {
            if (atlas == null)
            {
                string[] spriteNames = new string[]
                {
                    "Collapse",
                    "Expand",
                    "Collapse-Inverted",
                    "Expand-Inverted",
                    "SubcategoriesPanel",
                    "GenericTabHovered75",
                    "GenericTabHovered50",
                    "GenericTabHovered25",
                    "GenericTabHovered",
                    "Servicebar",
                    "Servicebar90",
                    "Servicebar80",
                    "Servicebar70",
                    "Servicebar60",
                    "Servicebar50",
                    "Servicebar25"
                };

                atlas = ResourceLoader.CreateTextureAtlas("YetAnotherToolbarAtlas", spriteNames, "YetAnotherToolbar.Icons.");

            }
            return atlas;
        }

        private static bool IsAssemblyEnabled(string assemblyName)
        {
            foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo())
            {
                foreach (Assembly assembly in plugin.GetAssemblies())
                {
                    if (assembly.GetName().Name.ToLower() == assemblyName)
                    {
                        Debugging.Message($"Found enabled mod: {assemblyName}. Yet Another Toolbar patch will be applied");
                        return plugin.isEnabled;
                    }
                }
            }
            return false;
        }

        public void Expand()
        {
            UpdateScale(1.0f);
            UpdateLayout(Settings.numOfRows, Settings.numOfCols);
            UpdateScale(Settings.toolbarScale);
            UpdatePanelPosition();
        }

        public void Collapse()
        {
            UpdateScale(1.0f);
            UpdateLayout(1, Settings.numOfCols);
            UpdateScale(Settings.toolbarScale);
            UpdatePanelPosition();
        }

        public void UpdateScale(float scaleFactor)
        {
            tsContainer.transform.localScale = new Vector2(scaleFactor, scaleFactor);
        }

        public void ResetScale()
        {
            tsContainer.transform.localScale = new Vector2(1.0f, 1.0f);
        }

        public void RestoreScale()
        {
            tsContainer.transform.localScale = new Vector2(Settings.toolbarScale, Settings.toolbarScale);
        }

        public void ToggleMenuVisibility()
        {
            if (!this.hideMenuFlag)
            {
                this.hideMenuFlag = true;
                lastMenuPosition = tsContainer.relativePosition;
                tsContainer.relativePosition = new Vector2(-50000, -50000);

                if (Settings.expanded) mainButton.normalFgSprite = "Collapse-Inverted";
                else mainButton.normalFgSprite = "Expand-Inverted";
            }
            else
            {
                this.hideMenuFlag = false;
                tsContainer.relativePosition = this.lastMenuPosition;
                if (Settings.expanded) mainButton.normalFgSprite = "Collapse";
                else mainButton.normalFgSprite = "Expand";
            }
        }

        public void CheckMenuVisibility()
        {
            if (!this.hideMenuFlag) return;
            ToggleMenuVisibility();
        }

        public void UpdatePanelPosition()
        {
            CheckMenuVisibility();
            UIView view = UIView.GetAView();
            float x = (596f * view.GetScreenResolution().x / 1920f) + Settings.horizontalOffset;
            float y = -110f + Settings.verticalOffset;

            if (Settings.expanded)
            {
                tsContainer.relativePosition = new Vector2(x, y - (104f * (Settings.numOfRows - 1)) + (104f * (Settings.numOfRows) * (1 - Settings.toolbarScale)));
            }
            else
            {
                tsContainer.relativePosition = new Vector2(x, y + (104f * (1 - Settings.toolbarScale)));
            }

        }


        private void UpdateLayout(int numOfRows, int numOfCols)
        {
            CheckMenuVisibility();
            try
            {
                UITabContainer gtsContainer;

                tsContainer.height = Mathf.Round(104f * numOfRows) + 1;
                tsContainer.width = Mathf.Round(859f - 763f + 109f * numOfCols) + 1;

                foreach (UIComponent toolPanel in tsContainer.components)
                {
                    if (toolPanel is UIPanel)
                    {
                        gtsContainer = toolPanel.GetComponentInChildren<UITabContainer>();

                        if (gtsContainer != null)
                        {
                            foreach (UIComponent tabPanel in gtsContainer.components)
                            {
                                tabPanel.height = tsContainer.height;
                                tabPanel.width = tsContainer.width;

                                UIScrollablePanel scrollablePanel = tabPanel.GetComponentInChildren<UIScrollablePanel>();
                                if (scrollablePanel != null)
                                {
                                    scrollablePanel.height = tsContainer.height;
                                    scrollablePanel.width = Mathf.Round(109f * numOfCols) + 1;
                                }

                                // don't mess further with Find It's panel
                                if (tabPanel.name == "FindItDefaultPanel") continue;

                                if (numOfRows > 1)
                                {
                                    UIScrollbar horizontalScrollbar = tabPanel.GetComponentInChildren<UIScrollbar>();
                                    if (horizontalScrollbar != null)
                                    {
                                        horizontalScrollbar.value = 0;
                                    }

                                    if (scrollablePanel != null)
                                    {
                                        scrollablePanel.autoLayout = true;
                                        scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;
                                        scrollablePanel.wrapLayout = true;
                                        scrollablePanel.autoLayoutDirection = LayoutDirection.Horizontal;
                                        scrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
                                    }

                                    UIScrollbar verticalScrollbar;
                                    if (dictVerticalScrollbars.ContainsKey((UIPanel)tabPanel))
                                    {
                                        verticalScrollbar = dictVerticalScrollbars[(UIPanel)tabPanel];
                                        AdjustVerticalScrollbar(verticalScrollbar, tabPanel, scrollablePanel);
                                    }
                                    else
                                    {
                                        verticalScrollbar = CreateVerticalScrollbar((UIPanel)tabPanel, scrollablePanel);
                                        dictVerticalScrollbars[(UIPanel)tabPanel] = verticalScrollbar;
                                    }
                                    verticalScrollbar.Show();
                                }
                                else
                                {
                                    if (scrollablePanel != null)
                                    {
                                        scrollablePanel.autoLayout = true;
                                        scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;
                                        scrollablePanel.wrapLayout = false;
                                        scrollablePanel.autoLayoutDirection = LayoutDirection.Horizontal;
                                        scrollablePanel.scrollWheelDirection = UIOrientation.Horizontal;
                                        scrollablePanel.ScrollToLeft();
                                    }

                                    UIScrollbar verticalScrollbar;
                                    if (dictVerticalScrollbars.ContainsKey((UIPanel)tabPanel))
                                    {
                                        verticalScrollbar = dictVerticalScrollbars[(UIPanel)tabPanel];
                                        AdjustVerticalScrollbar(verticalScrollbar, tabPanel, scrollablePanel);
                                    }
                                    else
                                    {
                                        verticalScrollbar = CreateVerticalScrollbar((UIPanel)tabPanel, scrollablePanel);
                                        dictVerticalScrollbars[(UIPanel)tabPanel] = verticalScrollbar;
                                    }
                                    dictVerticalScrollbars[(UIPanel)tabPanel].Hide();

                                    UIScrollbar horizontalScrollbar = tabPanel.GetComponentInChildren<UIScrollbar>();
                                    if (horizontalScrollbar != null)
                                    {
                                        if (horizontalScrollbar.decrementButton != null)
                                        {
                                            horizontalScrollbar.decrementButton.relativePosition = new Vector3(horizontalScrollbar.decrementButton.relativePosition.x, horizontalScrollbar.height / 2f - 16f);
                                        }
                                        if (horizontalScrollbar.incrementButton != null)
                                        {
                                            horizontalScrollbar.incrementButton.relativePosition = new Vector3(horizontalScrollbar.incrementButton.relativePosition.x, horizontalScrollbar.height / 2f - 16f);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

                if (isFindItEnabled) FindItLayoutPatch();
                if (isRICOEnabled && (!isEditorMode)) PloppableRICOLayoutPatch(numOfRows, numOfCols);

            }
            catch (Exception ex)
            {
                Debugging.Message("UpdateLayout() - " + ex.Message);
            }
        }

        private void FindItLayoutPatch()
        {
            try
            {
                GameObject findItObject = GameObject.Find("FindItDefaultPanel");
                if (findItObject == null) return;
                UIComponent finditDefaultPanel = findItObject.GetComponent<UIComponent>();

                if (finditDefaultPanel != null)
                {
                    UIComponent finditScrollablePanel = finditDefaultPanel.Find("ScrollablePanel").GetComponent<UIComponent>();

                    if (finditScrollablePanel != null)
                    {
                        UIScrollablePanel scrollablePanel = (UIScrollablePanel)finditScrollablePanel;

                        scrollablePanel.wrapLayout = true;
                        scrollablePanel.autoLayout = true;
                        scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;
                    }

                    UIComponent finditScrollbar = finditDefaultPanel.Find("UIScrollbar").GetComponent<UIComponent>();

                    if (finditScrollbar != null)
                    {
                        UIScrollbar scrollbar = (UIScrollbar)finditScrollbar;

                        UIComponent finditSlicedSpriteTrack = finditScrollbar.Find("UISlicedSprite").GetComponent<UIComponent>();

                        if (finditSlicedSpriteTrack != null)
                        {
                            UISlicedSprite slicedSpriteTrack = (UISlicedSprite)finditSlicedSpriteTrack;

                            slicedSpriteTrack.height = tsContainer.height;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugging.Message("FindItLayoutPatch() - " + ex.Message);
            }
        }

        private void PloppableRICOLayoutPatch(int numOfRows, int numOfCols)
        {
            try
            {
                UIComponent panel = GameObject.Find("PloppableBuildingPanel").GetComponent<UIComponent>();

                if (panel != null)
                {
                    UIPanel p = panel as UIPanel;
                    UIScrollablePanel scrollablePanel;

                    foreach (UIComponent comp in panel.components)
                    {
                        if (comp is UIScrollablePanel)
                        {
                            scrollablePanel = (UIScrollablePanel)comp;
                            p.height = tsContainer.height;
                            p.width = Mathf.Round(859f - 763f + 109f * numOfCols) + 1;

                            scrollablePanel.height = tsContainer.height;
                            scrollablePanel.width = Mathf.Round(109f * numOfCols) + 1;

                            if (numOfRows > 1)
                            {
                                scrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
                                scrollablePanel.autoLayoutDirection = LayoutDirection.Horizontal;
                            }
                            else
                            {
                                scrollablePanel.autoLayoutDirection = LayoutDirection.Horizontal;
                                scrollablePanel.scrollWheelDirection = UIOrientation.Horizontal;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugging.Message("PloppableRICOLayoutPatch() - " + ex.Message);
            }
        }

        public void UpdateMainPanelBackground()
        {
            UITabContainer gtsContainer;
            foreach (UIComponent toolPanel in tsContainer.components)
            {
                if (toolPanel is UIPanel)
                {
                    gtsContainer = toolPanel.GetComponentInChildren<UITabContainer>();

                    if (gtsContainer != null)
                    {
                        gtsContainer.atlas = YetAnotherToolbar.atlas;
                        switch (Settings.backgroundOption)
                        {
                            case 0:
                                gtsContainer.backgroundSprite = "SubcategoriesPanel"; // original
                                break;
                            case 1:
                                gtsContainer.backgroundSprite = "Servicebar"; // dark 100%
                                break;
                            case 2:
                                gtsContainer.backgroundSprite = "Servicebar90"; // dark 90%
                                break;
                            case 3:
                                gtsContainer.backgroundSprite = "Servicebar80"; // dark 80%
                                break;
                            case 4:
                                gtsContainer.backgroundSprite = "Servicebar70"; // dark 70%
                                break;
                            case 5:
                                gtsContainer.backgroundSprite = "Servicebar60"; // dark 60%
                                break;
                            case 6:
                                gtsContainer.backgroundSprite = "Servicebar50"; // dark 50%
                                break;
                            case 7:
                                gtsContainer.backgroundSprite = "Servicebar25"; // dark 25%
                                break;
                            case 8:
                                gtsContainer.backgroundSprite = ""; // dark 0%
                                break;
                            default:
                                gtsContainer.backgroundSprite = "SubcategoriesPanel";
                                break;
                        }

                        if (isRICOEnabled && (!isEditorMode))
                        {
                            UIComponent component = GameObject.Find("PloppableBuildingPanel").GetComponent<UIComponent>();
                            if (component != null)
                            {
                                UIPanel panel = component as UIPanel;
                                panel.atlas = gtsContainer.atlas;
                                panel.backgroundSprite = gtsContainer.backgroundSprite;
                            }
                        }
                    }
                }
            }
        }

        public void UpdateThumbnailBarBackground()
        {
            if (thumbnailBar != null)
            {
                pauseOutline.size = pauseOutlineOriginalSize;
                thumbnailBar.atlas = YetAnotherToolbar.atlas;

                switch (Settings.thumbnailBarBackgroundOption)
                {
                    case 0:
                        thumbnailBar.atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
                        thumbnailBar.spriteName = "Servicebar"; // original
                        break;
                    case 1:
                        thumbnailBar.spriteName = "Servicebar"; // dark 100%
                        break;
                    case 2:
                        thumbnailBar.spriteName = "Servicebar90"; // dark 90%
                        break;
                    case 3:
                        thumbnailBar.spriteName = "Servicebar80"; // dark 80%
                        break;
                    case 4:
                        thumbnailBar.spriteName = "Servicebar70"; // dark 70%
                        break;
                    case 5:
                        thumbnailBar.spriteName = "Servicebar60"; // dark 60%
                        break;
                    case 6:
                        thumbnailBar.spriteName = "Servicebar50"; // dark 50%
                        break;
                    case 7:
                        thumbnailBar.spriteName = "Servicebar25"; // dark 25%
                        break;
                    case 8:
                        thumbnailBar.spriteName = ""; // dark 0%
                        pauseOutline.size = originalScreenSize;
                        break;
                    default:
                        thumbnailBar.spriteName = "Servicebar";
                        break;
                }
            }
        }

        public void UpdateTSBarBackground()
        {
            if (tsBar != null)
            {
                tsBar.atlas = YetAnotherToolbar.atlas;
                switch (Settings.tsBarBackgroundOption)
                {
                    case 0:
                        tsBar.atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
                        tsBar.spriteName = "Toolbar";
                        break;
                    case 1:
                        tsBar.spriteName = "Servicebar"; // dark 100%
                        break;
                    case 2:
                        tsBar.spriteName = "Servicebar90"; // dark 90%
                        break;
                    case 3:
                        tsBar.spriteName = "Servicebar80"; // dark 80%
                        break;
                    case 4:
                        tsBar.spriteName = "Servicebar70"; // dark 70%
                        break;
                    case 5:
                        tsBar.spriteName = "Servicebar60"; // dark 60%
                        break;
                    case 6:
                        tsBar.spriteName = "Servicebar50"; // dark 50%
                        break;
                    case 7:
                        tsBar.spriteName = "Servicebar25"; // dark 25%
                        break;
                    case 8:
                        tsBar.spriteName = ""; // dark 0%
                        break;
                    default:
                        tsBar.atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
                        tsBar.spriteName = "Toolbar";
                        break;
                }
            }
        }

        public void UpdateInfoPanelBackground()
        {
            if (infoPanel != null)
            {
                infoPanel.atlas = YetAnotherToolbar.atlas;
                switch (Settings.infoPanelBackgroundOption)
                {
                    case 0:
                        infoPanel.atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
                        infoPanel.backgroundSprite = "Servicebar";
                        break;
                    case 1:
                        infoPanel.backgroundSprite = "Servicebar"; // dark 100%
                        break;
                    case 2:
                        infoPanel.backgroundSprite = "Servicebar90"; // dark 90%
                        break;
                    case 3:
                        infoPanel.backgroundSprite = "Servicebar80"; // dark 80%
                        break;
                    case 4:
                        infoPanel.backgroundSprite = "Servicebar70"; // dark 70%
                        break;
                    case 5:
                        infoPanel.backgroundSprite = "Servicebar60"; // dark 60%
                        break;
                    case 6:
                        infoPanel.backgroundSprite = "Servicebar50"; // dark 50%
                        break;
                    case 7:
                        infoPanel.backgroundSprite = "Servicebar25"; // dark 25%
                        break;
                    case 8:
                        infoPanel.backgroundSprite = ""; // dark 0%
                        break;
                    default:
                        infoPanel.atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
                        infoPanel.backgroundSprite = "Servicebar";
                        break;
                }
            }
        }

        private static UIScrollbar CreateVerticalScrollbar(UIPanel panel, UIScrollablePanel scrollablePanel)
        {
            UIScrollbar verticalScrollbar = panel.AddUIComponent<UIScrollbar>();
            verticalScrollbar.name = "VerticalScrollbar";
            verticalScrollbar.width = 20f;
            verticalScrollbar.height = panel.height;
            verticalScrollbar.orientation = UIOrientation.Vertical;
            verticalScrollbar.pivot = UIPivotPoint.BottomLeft;
            verticalScrollbar.AlignTo(panel, UIAlignAnchor.TopRight);
            verticalScrollbar.minValue = 0;
            verticalScrollbar.value = 0;
            verticalScrollbar.incrementAmount = 104;
            verticalScrollbar.autoHide = true;

            UISlicedSprite trackSprite = verticalScrollbar.AddUIComponent<UISlicedSprite>();
            trackSprite.name = "trackSprite";
            trackSprite.relativePosition = Vector2.zero;
            trackSprite.autoSize = true;
            trackSprite.size = trackSprite.parent.size;
            trackSprite.fillDirection = UIFillDirection.Vertical;
            trackSprite.spriteName = ""; // "ScrollbarTrack";

            verticalScrollbar.trackObject = trackSprite;

            UISlicedSprite thumbSprite = trackSprite.AddUIComponent<UISlicedSprite>();
            thumbSprite.name = "thumbSprite";
            thumbSprite.relativePosition = Vector2.zero;
            thumbSprite.fillDirection = UIFillDirection.Vertical;
            thumbSprite.autoSize = true;
            thumbSprite.width = thumbSprite.parent.width - 8;
            thumbSprite.spriteName = "ScrollbarThumb";

            verticalScrollbar.thumbObject = thumbSprite;
            scrollablePanel.verticalScrollbar = verticalScrollbar;

            verticalScrollbar.eventValueChanged += (component, value) => {
                scrollablePanel.scrollPosition = new Vector2(0, value);
            };

            panel.eventMouseWheel += (component, eventParam) => {
                scrollablePanel.scrollPosition = new Vector2(0, scrollablePanel.scrollPosition.y - (int)eventParam.wheelDelta * verticalScrollbar.incrementAmount);
            };

            return verticalScrollbar;
        }

        private static void AdjustVerticalScrollbar(UIScrollbar verticalScrollbar, UIComponent tabPanel, UIScrollablePanel scrollablePanel)
        {
            verticalScrollbar.width = 20f;
            verticalScrollbar.height = tabPanel.height;
            verticalScrollbar.orientation = UIOrientation.Vertical;
            verticalScrollbar.pivot = UIPivotPoint.BottomLeft;
            verticalScrollbar.AlignTo(tabPanel, UIAlignAnchor.TopRight);
            verticalScrollbar.minValue = 0;
            verticalScrollbar.value = 0;
            verticalScrollbar.incrementAmount = 104;
            verticalScrollbar.autoHide = true;

            UISlicedSprite trackSprite = (UISlicedSprite)verticalScrollbar.Find("trackSprite");
            trackSprite.relativePosition = Vector2.zero;
            trackSprite.autoSize = true;
            trackSprite.size = trackSprite.parent.size;
            trackSprite.fillDirection = UIFillDirection.Vertical;
            trackSprite.spriteName = ""; // "ScrollbarTrack";

            UISlicedSprite thumbSprite = (UISlicedSprite)verticalScrollbar.Find("thumbSprite");
            thumbSprite.relativePosition = Vector2.zero;
            thumbSprite.fillDirection = UIFillDirection.Vertical;
            thumbSprite.autoSize = true;
            thumbSprite.width = thumbSprite.parent.width - 8;
            thumbSprite.spriteName = "ScrollbarThumb";

            verticalScrollbar.thumbObject = thumbSprite;
            scrollablePanel.verticalScrollbar = verticalScrollbar;
        }

        public void SetAdvisorButtonVisibility()
        {
            try
            {
                UIMultiStateButton advisorButton = UIView.Find("AdvisorButton") as UIMultiStateButton;
                if (advisorButton == null) return;

                if (Settings.hideAdvisorButton)
                    advisorButton.Hide();
                else
                    advisorButton.Show();
            }
            catch (Exception ex)
            {
                Debugging.Message("SetAdvisorButtonVisibility() - " + ex.Message);
            }
        }

        /*
        public void SetFilterPanelsVisibility()
        {
            try
            {
                UITabContainer gtsContainer;

                foreach (UIComponent toolPanel in tsContainer.components)
                {
                    if (toolPanel is UIPanel)
                    {
                        gtsContainer = toolPanel.GetComponentInChildren<UITabContainer>();

                        if (gtsContainer != null)
                        {
                            foreach (UIComponent tabPanel in gtsContainer.components)
                            {
                                UIPanel filterPanel = tabPanel.Find("FilterPanel") as UIPanel;
                                if (hideFilterPanels)
                                    filterPanel.Hide();
                                else
                                    filterPanel.Show();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugging.Message("HideFilterPanels() - " + ex.Message);
            }
        }
        */

    }

}
