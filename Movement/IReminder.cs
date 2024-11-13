using UnityEngine;

namespace Movement
{
    internal interface IReminder
    {
        float ReminderSqr { get; }
        Vector2 Reminder { get; }
    }
}