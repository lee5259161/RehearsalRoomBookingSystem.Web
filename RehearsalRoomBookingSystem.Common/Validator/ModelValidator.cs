using System.ComponentModel.DataAnnotations;

namespace RehearsalRoomBookingSystem.Common.Validator
{
    /// <summary>
    /// Class ModelValidator.
    /// </summary>
    public class ModelValidator
    {
        /// <summary>
        /// Validates the specified model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="parameterName">parameterName</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Validate<T>(T model, string parameterName) where T : class
        {
            if (model is null)
            {
                if (string.IsNullOrWhiteSpace(parameterName))
                {
                    parameterName = nameof(model);
                }
                throw new ArgumentNullException(paramName: parameterName, message: $"The value '{parameterName}' cannot be null.");
            }

            var errors = new List<ValidationResult>();
            var validation =
                System.ComponentModel.DataAnnotations
                      .Validator.TryValidateObject(model, new ValidationContext(model), errors, true);

            if (validation.Equals(false) && errors.Any().Equals(true))
            {
                var error = errors.FirstOrDefault();
                throw new ArgumentException(message: error.ErrorMessage, paramName: error.MemberNames.FirstOrDefault());
            }
        }
    }
}