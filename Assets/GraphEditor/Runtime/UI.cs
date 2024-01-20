﻿using System.Collections;
using System.Linq;
using UnityEngine;

namespace GraphEditor.Runtime
{
    public class UI : MonoBehaviour
    {
        public static UI Instance { get; private set; }
        
        public bool IsWaitingMouseClick { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError($"Больше чем два {nameof(UI)} на сцене");
                Destroy(gameObject);
            }
        }
        
        public void CreateNode()
        {
            StopAllCoroutines();
            StartCoroutine(WaitMouseAndCrateNode());
        }
        
        public void ConnectOrDisconnectNodes()
        {
            var activeNodes = GraphEditorRoot.Instance.NodeSelector.SelectedNodes.ToArray();
            if (activeNodes.Length < 2)
                Debug.LogError("Not enough nodes to execute connect or disconnect");
            else if (activeNodes.Length > 2)
                Debug.LogError("Too many nodes");
            else
                GraphEditorRoot.Instance.GraphTool.ConnectOrDisconnectNodes(activeNodes[0], activeNodes[1]);
        }
        
        public void RegenerateGraph()
        {
            GraphEditorRoot.Instance.RegenerateGraph();
        }

        private IEnumerator WaitMouseAndCrateNode()
        {
            IsWaitingMouseClick = true;
            while (!Input.GetMouseButtonUp(0) || CameraController.PointerIsOverUI())
                yield return null;

            IsWaitingMouseClick = false;
            GraphEditorRoot.Instance.GraphTool.CreateNode(CameraController.MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}