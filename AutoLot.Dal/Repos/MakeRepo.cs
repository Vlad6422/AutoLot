﻿using AutoLot.Dal.EfStructures;
using AutoLot.Dal.Repos.Base;
using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Dal.Repos
{
    public class MakeRepo : BaseRepo<Make>, IMakeRepo
    {
        public MakeRepo(ApplicationDbContext context) : base(context)
        {
        }

        internal MakeRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public override IEnumerable<Make> GetAll()
            => Table.OrderBy(m => m.Name);

        public override IEnumerable<Make> GetAllIgnoreQueryFilters()
            => Table.IgnoreQueryFilters().OrderBy(m => m.Name);

    }
}