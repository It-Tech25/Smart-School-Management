
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
 
﻿using SmartSchool.Models.DTO;
 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchool.Service
    {
        public interface ITeachersServices
        { 
        List<TeachersDto> GetAllAsync(int id);
        GenericResponse AddAsync(TeachersDto dto, int id);
        GenericResponse UpdateAsync(TeachersDto dto, int id);
        GenericResponse DeleteAsync(int id, int logid);
     
            Task<IEnumerable<TeachersDto>> GetAllAsync();
            Task<TeachersDto> GetByIdAsync(int id);
            Task<TeachersDto> CreateAsync(TeachersDto dto);
            Task<TeachersDto> UpdateAsync(TeachersDto dto);
            Task<bool> DeleteAsync(int id);
        } 
    }

