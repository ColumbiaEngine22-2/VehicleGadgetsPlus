﻿namespace VehicleGadgetsPlus.VehicleGadgets
{
    using System;

    using Rage;

    using VehicleGadgetsPlus.VehicleGadgets.XML;

    internal abstract class VehicleGadget : IDisposable
    {
        public Vehicle Vehicle { get; }
        public VehicleGadgetEntry DataEntry { get; }
        public bool IsDisposed { get; private set; }

        protected VehicleGadget(Vehicle vehicle, VehicleGadgetEntry dataEntry)
        {
            Vehicle = vehicle;
            DataEntry = dataEntry;
        }

        ~VehicleGadget()
        {
            Dispose(false);
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public abstract void Update(bool isPlayerIn);


        public static VehicleGadget[] GetGadgetsForVehicle(Vehicle vehicle)
        {
            if(Plugin.VehicleConfigsByModel.TryGetValue(vehicle.Model, out VehicleConfig config))
            {
                VehicleGadget[] exts = new VehicleGadget[config.Gadgets.Length];
                for (int i = 0; i < config.Gadgets.Length; i++)
                {
                    VehicleGadgetEntry entry = config.Gadgets[i];
                    exts[i] = (VehicleGadget)Activator.CreateInstance(entry.GadgetType, vehicle, entry);
                }
                return exts;
            }

            return null;
        }
    }
}
