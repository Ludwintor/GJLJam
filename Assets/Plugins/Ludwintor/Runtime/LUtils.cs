using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Ludwintor.Tools
{
    public static class LUtils
    {
        /// <summary>
        /// Uses unity <see cref="EventTrigger"/> class to add UI events
        /// </summary>
        /// <param name="obj">UI object that gonna respond on events</param>
        /// <param name="type">Event type</param>
        /// <param name="action">Callback on event rises</param>
        public static void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            if (!obj.TryGetComponent(out EventTrigger trigger))
                trigger = obj.AddComponent<EventTrigger>();

            EventTrigger.Entry eventTrigger = new EventTrigger.Entry
            {
                eventID = type
            };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        #region CreateWorldText()
        public static TextMeshPro CreateWorldText(string text) => CreateWorldText(text, null, default, 40, Color.white, TextAlignmentOptions.Center, 5000);
        public static TextMeshPro CreateWorldText(string text, Transform parent) => CreateWorldText(text, parent, default, 40, Color.white, TextAlignmentOptions.Center, 5000);
        public static TextMeshPro CreateWorldText(string text, Transform parent, Vector3 localPosition) => CreateWorldText(text, parent, localPosition, 40, Color.white, TextAlignmentOptions.Center, 5000);
        public static TextMeshPro CreateWorldText(string text, Transform parent, Vector3 localPosition, float fontSize) => CreateWorldText(text, parent, localPosition, fontSize, Color.white, TextAlignmentOptions.Center, 5000);

        public static TextMeshPro CreateWorldText(string text, Transform parent, Vector3 localPosition, float fontSize, Color color, TextAlignmentOptions textAlignment, int sortingOrder)
        {
            GameObject go = new GameObject("World Text");
            Transform transform = go.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMeshPro textMesh = go.AddComponent<TextMeshPro>();
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMesh;
        }
        #endregion

        public static Vector2 GetMouseWorldPosition() => GetMouseWorldPosition(Input.mousePosition, Camera.main);
        public static Vector2 GetMouseWorldPosition(Camera camera) => GetMouseWorldPosition(Input.mousePosition, camera);

        public static Vector2 GetMouseWorldPosition(Vector3 screenPosition, Camera camera)
        {
            return GetMouseWorldPosition3D(screenPosition, camera);
        }

        public static Vector3 GetMouseWorldPosition3D() => GetMouseWorldPosition3D(Input.mousePosition, Camera.main);
        public static Vector3 GetMouseWorldPosition3D(Camera camera) => GetMouseWorldPosition3D(Input.mousePosition, camera);

        public static Vector3 GetMouseWorldPosition3D(Vector3 screenPosition, Camera camera)
        {
            return camera.ScreenToWorldPoint(screenPosition);
        }

        public static Vector2 GetRandomDirection() => new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}