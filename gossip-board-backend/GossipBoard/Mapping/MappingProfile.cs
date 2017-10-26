using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GossipBoard.Dto;
using GossipBoard.Models;
using AutoMapper;

namespace GossipBoard.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
 
            CreateMap<Post, PostDto>();
          
        }
    }
}
