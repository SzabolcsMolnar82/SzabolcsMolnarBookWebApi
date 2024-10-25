using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SzabolcsMolnarBookWebApi.DTOs;
using SzabolcsMolnarBookWebApi.Entities;

namespace SzabolcsMolnarBookWebApi.DTOs
{

    public class PostAuthorDto
    {
        public string Name
        {
            get; set;
        }
        public DateTime BirthDate
        {
            get; set;
        }
    }





}



