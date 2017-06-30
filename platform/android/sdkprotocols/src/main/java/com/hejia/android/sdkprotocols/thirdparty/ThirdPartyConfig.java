package com.hejia.android.sdkprotocols.thirdparty;

import android.util.SparseArray;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by hejia on 2017/6/28.
 */

public class ThirdPartyConfig {

    private List<Thirdparty> thirdparty;
    private List<Integer> install;

    public List<Thirdparty> getThirdparty() {
        return thirdparty;
    }

    public void setThirdparty(List<Thirdparty> thirdparty) {
        this.thirdparty = thirdparty;
    }

    public List<Integer> getInstall() {
        return install;
    }

    public void setInstall(List<Integer> install) {
        this.install = install;
    }

    public List<Thirdparty> getInstallThirdparties() {
        List<Thirdparty> list = new ArrayList<Thirdparty>();
        SparseArray<Thirdparty> thirdpartyMap = new SparseArray<Thirdparty>();
        for (Thirdparty p : thirdparty) {
            int provider = p.getProvider();
            if (thirdpartyMap.get(provider, null) != null)
                throw new AssertionError("thirdparty.json中存在重复的provider");
            thirdpartyMap.put(provider, p);
        }
        for (int provider : install) {
            Thirdparty p = thirdpartyMap.get(provider, null);
            if (p != null)
                list.add(p);
        }
        return list;
    }

    public static class Thirdparty {

        private int provider;
        private String name;
        private String className;
        private String folder;
        private String params;
        private String type;
        private String exit;

        public int getProvider() {
            return provider;
        }

        public void setProvider(int provider) {
            this.provider = provider;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getClassName() {
            return className;
        }

        public void setClassName(String className) {
            this.className = className;
        }

        public String getFolder() {
            return folder;
        }

        public void setFolder(String folder) {
            this.folder = folder;
        }

        public String getParams() {
            return params;
        }

        public void setParams(String params) {
            this.params = params;
        }

        public String getType() {
            return type;
        }

        public void setType(String type) {
            this.type = type;
        }

        public String getExit() {
            return exit;
        }

        public void setExit(String exit) {
            this.exit = exit;
        }
    }
}
