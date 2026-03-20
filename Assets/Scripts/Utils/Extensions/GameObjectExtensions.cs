using UnityEngine;
namespace UnityUtils
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// This method is used to hide the GameObject in the Hierarchy view.
        /// Hello
        /// </summary>
        /// <param name="gameObject"></param>
        public static void HideInHierarchy(this GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        /// <summary>
        /// Returns the object itself if it exists, null otherwise.
        /// </summary>
        /// <remarks>
        /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
        /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
        /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
        /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
        /// </remarks>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object being checked.</param>
        /// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
        
        public static GameObject OrNullOrInactive(this GameObject obj) => obj != null && obj.activeSelf ? obj : null;

        /// <summary>
        /// Activates the GameObject associated with the MonoBehaviour and returns the instance.
        /// </summary>
        /// <typeparam name="T">The type of the MonoBehaviour.</typeparam>
        /// <param name="obj">The MonoBehaviour whose GameObject will be activated.</param>
        /// <returns>The instance of the MonoBehaviour.</returns>
        public static T SetActive<T>(this T obj) where T : MonoBehaviour
        {
            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Deactivates the GameObject associated with the MonoBehaviour and returns the instance.
        /// </summary>
        /// <typeparam name="T">The type of the MonoBehaviour.</typeparam>
        /// <param name="obj">The MonoBehaviour whose GameObject will be deactivated.</param>
        /// <returns>The instance of the MonoBehaviour.</returns>
        public static T SetInactive<T>(this T obj) where T : MonoBehaviour
        {
            obj.gameObject.SetActive(false);
            return obj;
        }
    }
}
