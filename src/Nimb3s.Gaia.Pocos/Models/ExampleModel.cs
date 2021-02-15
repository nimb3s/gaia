using System;
using System.ComponentModel.DataAnnotations;

namespace Nimb3s.Gaia.Pocos.Models
{

    /// <summary>
    /// The automation request
    /// </summary>
    public class ExampleModel
    {
        /// <summary>
        /// The automation job name
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
