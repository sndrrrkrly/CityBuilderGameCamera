using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace Project.Star {
    public class PSPlayerCameraFollowObject : MonoBehaviour {
        public void OnMouseDown () {
            PSPlayerCameraController.instance.followTransform = transform;
        }
    }
}