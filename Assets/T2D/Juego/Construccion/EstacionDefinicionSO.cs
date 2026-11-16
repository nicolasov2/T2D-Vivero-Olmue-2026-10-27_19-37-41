using T2D.Dominio;
using UnityEngine;

namespace T2D.Juego
{
    /// <summary>UT 1687. Editable data of a kind of station; the default is the fixed SBC camera of the mockups.</summary>
    [CreateAssetMenu(menuName = "T2D/Estación", fileName = "Estacion")]
    public sealed class EstacionDefinicionSO : ScriptableObject
    {
        [SerializeField] private string nombre = "Cámara SBC";
        [SerializeField, Min(0)] private int costoAgua = 60;
        [SerializeField, Min(0)] private int costoEnergia = 40;
        [SerializeField, Min(1)] private int radioCobertura = 2;
        [SerializeField, Min(1)] private int integridadMaxima = 3;

        private DefinicionEstacion _definicion;

        /// <summary>
        /// Domain definition. The same instance is returned every time so the selection can compare it;
        /// it is rebuilt when the asset is edited.
        /// </summary>
        public DefinicionEstacion Definicion =>
            _definicion ??= new DefinicionEstacion(nombre, new Costo(costoAgua, costoEnergia), radioCobertura, integridadMaxima);

        private void OnEnable() => _definicion = null;

        private void OnValidate() => _definicion = null;
    }
}
