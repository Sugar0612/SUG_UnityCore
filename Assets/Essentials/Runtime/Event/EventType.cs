using UnityEngine;

namespace SUG.Essentials.Event
{
    public class PlayerDiedEvent: IEventBusR<float, int> { }
    public class PlayerAttackEvent : IEventBusR<float> { }
}