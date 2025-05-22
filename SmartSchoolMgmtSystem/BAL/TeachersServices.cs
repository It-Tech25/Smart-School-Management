
using SmartSchool.Utilities;
using SmartSchool.Models.DTO;
 
﻿using SmartSchool.Models.DTO; 
using SmartSchool.Models.Entity;
using SmartSchool.Repository;

namespace SmartSchool.Service
    {
        public class TeachersServices : ITeachersServices
        {
            private readonly ITeachersRepo _repo;

            public TeachersServices(ITeachersRepo repo)
            {
                _repo = repo;
            }
         
            public List<TeachersDto> GetAllAsync(int id)
            {
                var teachers =  _repo.GetAllAsync(id);
                return teachers;
            }


            public GenericResponse AddAsync(TeachersDto dto, int id)
            {
                
                var added = _repo.AddAsync(dto, id);
                return added;
            }

            public GenericResponse UpdateAsync(TeachersDto dto, int id)
            {
                
                var updated =  _repo.UpdateAsync(dto,id);
                return updated;
            }

            public GenericResponse   DeleteAsync(int id, int logid)
            {
                return  _repo.DeleteAsync(id,logid);
            }

             
            public async Task<IEnumerable<TeachersDto>> GetAllAsync()
            {
                var teachers = await _repo.GetAllAsync();
                return teachers.Select(t => MapToDto(t));
            }

            public async Task<TeachersDto> GetByIdAsync(int id)
            {
                var teacher = await _repo.GetByIdAsync(id);
                return teacher == null ? null : MapToDto(teacher);
            }

            public async Task<TeachersDto> CreateAsync(TeachersDto dto)
            {
                var entity = MapToEntity(dto);
                var added = await _repo.AddAsync(entity);
                return MapToDto(added);
            }

            public async Task<TeachersDto> UpdateAsync(TeachersDto dto)
            {
                var entity = MapToEntity(dto);
                var updated = await _repo.UpdateAsync(entity);
                return MapToDto(updated);
            }

            public async Task<bool> DeleteAsync(int id)
            {
                return await _repo.DeleteAsync(id);
            }

            // Mapping Helpers
            private TeachersDto MapToDto(TeachersEntity e) => new TeachersDto
            {
                TeacherId = e.TeacherId,
                UserId = e.UserId,
                SchoolId = e.SchoolId,
                ClassId = e.ClassId,
                DirectionId = e.DirectionId,
                SubjectId = e.SubjectId
            };

            private TeachersEntity MapToEntity(TeachersDto dto) => new TeachersEntity
            {
                TeacherId = dto.TeacherId,
                UserId = dto.UserId,
                SchoolId = dto.SchoolId,
                ClassId = dto.ClassId,
                DirectionId = dto.DirectionId,
                SubjectId = dto.SubjectId
            }; 
        }
    }
