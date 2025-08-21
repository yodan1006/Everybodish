using System.Collections.Generic;
using Grab.Runtime;
using UnityEngine;
namespace Grab.Data
{
    public interface IProximityGrabber : IRigidbodyGrabber
    {
        Collider[] GetCollidersInArea();
        bool TryGrabClosestAvailable(List<IGrabable> grabables);
    }
}