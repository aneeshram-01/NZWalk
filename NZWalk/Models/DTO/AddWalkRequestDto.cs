﻿using System.ComponentModel.DataAnnotations;

namespace NZWalk.Models.DTO
{
    public class AddWalkRequestDto
    {
        [Required] //Model Validation
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [Range(0,50)]
        public double LengthInKm { get; set; }
        public string? WalkImgUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
