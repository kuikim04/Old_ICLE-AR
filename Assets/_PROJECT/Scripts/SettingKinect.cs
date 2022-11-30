using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SettingKinect : MonoBehaviour
{
    void Awake()
    {
        var folder = Path.Combine(Application.dataPath, @"..\");

        //Application.OpenURL(folder + "kinect_config.json");

        var kinect = GetComponent<KinectManager>();
        var path = folder + "kinect_config.json";
        var file_exist = File.Exists(path);
        if (file_exist)
        {
            // read
            var json = File.ReadAllText(path);
            var config = JsonUtility.FromJson<Config>(json);
            kinect.sensorHeight = config.height;
            kinect.sensorAngle = config.angle;
        }
        else
        {
            // write default file.
            var config = new Config() { height = kinect.sensorHeight, angle = kinect.sensorAngle };
            var json = JsonUtility.ToJson(config);
            File.WriteAllText(path, json);
        }
    }

    [System.Serializable]
    public class Config
    {
        public float height;
        public float angle;
    }

}
