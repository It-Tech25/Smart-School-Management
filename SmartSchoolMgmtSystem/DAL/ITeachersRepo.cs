
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
using SmartSchool.Models.Entity;
 
﻿using SmartSchool.Models.Entity; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchool.Repository
{
    public interface ITeachersRepo
    { 
        List<TeachersDto> GetAllAsync(int id);
        GenericResponse AddAsync(TeachersDto dto, int id);
        GenericResponse UpdateAsync(TeachersDto dto, int id);
        GenericResponse DeleteAsync(int id,int logid); 
        Task<IEnumerable<TeachersEntity>> GetAllAsync();
        Task<TeachersEntity> GetByIdAsync(int id);
        Task<TeachersEntity> AddAsync(TeachersEntity teacher);
        Task<TeachersEntity> UpdateAsync(TeachersEntity teacher);
        Task<bool> DeleteAsync(int id); 
    }
}
