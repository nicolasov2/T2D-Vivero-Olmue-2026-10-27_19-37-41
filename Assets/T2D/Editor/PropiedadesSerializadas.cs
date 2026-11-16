using UnityEditor;
using UnityEngine;

namespace T2D.Herramientas
{
    /// <summary>Sets private [SerializeField] fields from editor code, failing loudly when a field name is wrong.</summary>
    internal static class PropiedadesSerializadas
    {
        /// <summary>Edits several fields of an object and saves them without adding Undo steps.</summary>
        public static void Editar(Object objeto, System.Action<SerializedObject> editar)
        {
            var serializado = new SerializedObject(objeto);
            editar(serializado);
            serializado.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>Field of a serialized object; a missing name is a bug in this tool.</summary>
        public static SerializedProperty Campo(SerializedObject serializado, string nombre) =>
            serializado.FindProperty(nombre) ??
            throw new System.InvalidOperationException($"No existe el campo serializado \"{nombre}\" en {serializado.targetObject.GetType().Name}.");

        /// <summary>Sets one object reference field.</summary>
        public static void Asignar(Object objeto, string campo, Object valor) =>
            Editar(objeto, serializado => Campo(serializado, campo).objectReferenceValue = valor);

        /// <summary>Replaces the content of an array field of object references.</summary>
        public static void AsignarLista(Object objeto, string campo, params Object[] valores) => Editar(objeto, serializado =>
        {
            var lista = Campo(serializado, campo);
            lista.arraySize = valores.Length;
            for (var i = 0; i < valores.Length; i++)
            {
                lista.GetArrayElementAtIndex(i).objectReferenceValue = valores[i];
            }
        });
    }
}
