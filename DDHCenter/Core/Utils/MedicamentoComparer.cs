using DDHCenter.Core.Models;
using System;
using System.Collections.Generic;

namespace DDHCenter.Core.Utils
{
    public class MedicamentoComparer : IEqualityComparer<Medicamento>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Medicamento x, Medicamento y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Id == y.Id;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Medicamento medicamento)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(medicamento, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductName = medicamento.Id.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName;
        }
    }
}
