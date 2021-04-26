using UnityEngine;

using Deblue.ObservingSystem;

namespace Deblue.LD48
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger : MonoBehaviour
    {
        public Handler<Player_Trigger> PlayerInTrigger = new Handler<Player_Trigger>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                PlayerInTrigger.Raise(new Player_Trigger());
            }
        }
    }

    public readonly struct Player_Trigger
    {
    }
}