using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class UserData {
    public string username;
    public int level;
    public int activeBuild;
    public int unitLimit;
    public Build[] builds;

    public string GetUsername() {
        return username;
    }

    public int GetLevel() {
        return level;
    }

    public Build GetBuild(int index) {
        return builds[index];
    }

    public Build[] GetBuilds() {
        return builds;
    }

    public Build GetActiveBuild() {
        return builds[activeBuild];
    }

    public int GetUnitLimit() {
        return unitLimit;
    }
}
