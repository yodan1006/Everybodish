using Grab.Runtime;
using System.Collections.Generic;
using UnityEngine;
namespace Grab.Data
{
    public interface IProximityGrabber : IRigidbodyGrabber
    {
        Collider[] GetCollidersInArea();
        bool TryGrabClosestAvailable(List<IGrabable> grabables);
    }
}