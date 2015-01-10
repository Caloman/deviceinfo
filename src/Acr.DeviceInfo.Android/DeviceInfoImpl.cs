﻿using System;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Telephony;
using B = Android.OS.Build;
using App = Android.App.Application;


namespace Acr.DeviceInfo {

    public class DeviceInfoImpl : IDeviceInfo {

        private readonly Lazy<string> appVersion;
        private readonly Lazy<string> deviceId;
        private readonly Lazy<int> screenHeight;
        private readonly Lazy<int> screenWidth;


        public DeviceInfoImpl() {
            this.appVersion = new Lazy<string>(() =>
                App
                    .Context
                    .ApplicationContext
                    .PackageManager
                    .GetPackageInfo(App.Context.PackageName, 0)
                    .VersionName
            );
            this.screenHeight = new Lazy<int>(() => {
                var d = Resources.System.DisplayMetrics;
                return (int)(d.HeightPixels / d.Density);
            });
            this.screenWidth = new Lazy<int>(() => {
                var d = Resources.System.DisplayMetrics;
                return (int)(d.WidthPixels / d.Density);
            });
            this.deviceId = new Lazy<string>(() => {
                var tel = (TelephonyManager)App.Context.GetSystemService(Context.TelephonyService);
                return tel.DeviceId;
            });
        }


        public string AppVersion {
            get { return this.appVersion.Value; }
        }


        public int ScreenHeight {
            get { return this.screenHeight.Value; }
        }


        public int ScreenWidth {
            get { return this.screenWidth.Value; }
        }


        public string DeviceId {
            get { return this.deviceId.Value; }
        }


        public string Manufacturer {
            get { return B.Manufacturer; }
        }


        public string Model {
            get { return B.Model; }
        }


        private string os;
        public string OperatingSystem {
            get {
                this.os = this.os ?? String.Format("{0} - SDK: {1}", B.VERSION.Release, B.VERSION.SdkInt);
                return this.os;
            }
        }


        public bool IsFrontCameraAvailable {
            get { return App.Context.ApplicationContext.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFront); }
        }


        public bool IsRearCameraAvailable {
            get { return App.Context.ApplicationContext.PackageManager.HasSystemFeature(PackageManager.FeatureCamera); }
        }


        public bool IsSimulator {
            get { return B.Product.Equals("google_sdk"); }
        }
    }
}
////var filter = new IntentFilter(Intent.ActionBatteryChanged);
////var battery = RegisterReceiver(null, filter);
////var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
////var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);
////this.BatteryPercentage = Convert.ToInt32(Math.Floor(level * 100D / scale));