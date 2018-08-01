using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AcademiaCodigoWarehouseApi.Exceptions {
    public class ValidationException : Exception {

        public ValidationException (ModelStateDictionary modelState) {
            if (modelState == null) {
                throw new ArgumentNullException (nameof (modelState));
            }

            var data = new Dictionary<string, IList<string>> ();
            foreach (var ms in modelState) {
                var key = ms.Key.ToLowerInvariant ();

                if (!data.TryGetValue (key, out var errors)) {
                    errors = new List<string> ();
                    data[key] = errors;
                }

                foreach (var e in ms.Value.Errors) {
                    errors.Add (e.ErrorMessage);
                }
            }
            Data = data;
        }

        public new IDictionary<string, IList<string>> Data { get; }
    }
}