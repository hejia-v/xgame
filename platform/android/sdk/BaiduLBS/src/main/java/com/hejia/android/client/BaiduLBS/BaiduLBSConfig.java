package com.hejia.android.client.BaiduLBS;

/**
 * Created by hejia on 2017/6/27.
 */

public class BaiduLBSConfig {

    /**
     * Option : {"locateMode":1,"coordinateSystem":1,"locateInterval":"3000","geofencelog":false,"poi":false,"describe":false,"director":false}
     * AutoNotify : {"autonotify_distance":100,"autonotify_time":60000,"sensitivity":1}
     */

    private Option option;
    private AutoNotify autoNotify;

    public Option getOption() {
        return option;
    }

    public void setOption(Option option) {
        this.option = option;
    }

    public AutoNotify getAutoNotify() {
        return autoNotify;
    }

    public void setAutoNotify(AutoNotify autoNotify) {
        this.autoNotify = autoNotify;
    }

    public static class Option {
        /**
         * locateMode : 1
         * coordinateSystem : 1
         * locateInterval : 3000
         * geofencelog : false
         * poi : false
         * describe : false
         * director : false
         */

        private int locateMode;
        private int coordinateSystem;
        private String locateInterval;
        private boolean geofencelog;
        private boolean poi;
        private boolean describe;
        private boolean director;

        public int getLocateMode() {
            return locateMode;
        }

        public void setLocateMode(int locateMode) {
            this.locateMode = locateMode;
        }

        public int getCoordinateSystem() {
            return coordinateSystem;
        }

        public void setCoordinateSystem(int coordinateSystem) {
            this.coordinateSystem = coordinateSystem;
        }

        public String getLocateInterval() {
            return locateInterval;
        }

        public void setLocateInterval(String locateInterval) {
            this.locateInterval = locateInterval;
        }

        public boolean isGeofencelog() {
            return geofencelog;
        }

        public void setGeofencelog(boolean geofencelog) {
            this.geofencelog = geofencelog;
        }

        public boolean isPoi() {
            return poi;
        }

        public void setPoi(boolean poi) {
            this.poi = poi;
        }

        public boolean isDescribe() {
            return describe;
        }

        public void setDescribe(boolean describe) {
            this.describe = describe;
        }

        public boolean isDirector() {
            return director;
        }

        public void setDirector(boolean director) {
            this.director = director;
        }
    }

    public static class AutoNotify {
        /**
         * autonotify_distance : 100
         * autonotify_time : 60000
         * sensitivity : 1
         */

        private int autonotify_distance;
        private int autonotify_time;
        private int sensitivity;

        public int getAutonotify_distance() {
            return autonotify_distance;
        }

        public void setAutonotify_distance(int autonotify_distance) {
            this.autonotify_distance = autonotify_distance;
        }

        public int getAutonotify_time() {
            return autonotify_time;
        }

        public void setAutonotify_time(int autonotify_time) {
            this.autonotify_time = autonotify_time;
        }

        public int getSensitivity() {
            return sensitivity;
        }

        public void setSensitivity(int sensitivity) {
            this.sensitivity = sensitivity;
        }
    }
}
