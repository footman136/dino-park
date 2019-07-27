using GameLogic.Core; // SimulationSettings

namespace GameLogic.Utils
{
    public static class QuantizationUtils
    {
        public static uint QuantizeAngle(float angle)
        {
            return (uint)(angle * SimulationSettings.AngleQuantisationFactor);
        }

        public static float DequantizeAngle(uint angle)
        {
            return angle / SimulationSettings.AngleQuantisationFactor;
        }
    }
}
