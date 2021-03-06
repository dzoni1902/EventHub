﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventHub.Core.Models;

namespace EventHub.Core.ViewModels
{
    public class EventFormViewModel
    {
        //view model is used for UI, to prevent adding new stuff and polute our domain classes
        //validations for UI fields go here, as attributes on props, then modification of
        //controller and view as well.
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }  //type is int - because of dropdown list (GenreId in DB)

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();

        public string Heading { get; set; }

        public string Action => (Id != 0) ? "Update" : "Create";

        //convert prop to method to avoid error caused by Reflection, when MVC calls Create
        //action and uses Reflection to recreate viewModel
        public DateTime GetDateTime() => DateTime.Parse(string.Format($"{Date}-{Time}"));
    }
}