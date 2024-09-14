using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ProjectClock.BusinessLogic.Dtos.Project.ProjectDtos;
using ProjectClock.BusinessLogic.Services.ProjectServices;
using ProjectClock.Database;
using ProjectClock.Database.Entities;
using System;
using System.Threading.Tasks;
using Xunit;


namespace ProjectClock.Tests.Services;

public class ProjectServicesTests
{
    //Create

    [Fact]
    public async Task Create_NewProject_ReturnsTrue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProjectClockDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectDatabase")
            .Options;

        using (var context = new ProjectClockDbContext(options))
        {
            var projectServices = new ProjectServices(context, Substitute.For<IMapper>());

            var createProjectDto = new CreateProjectDto
            {
                ProjectName = "New Project",
                OrganizationName = "Organization"
            };

            // Act
            var result = await projectServices.Create(createProjectDto);

            // Assert
            result.Should().BeTrue();
        }
    }

    [Fact]
    public async Task Create_ExistingProject_ReturnsFalse()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProjectClockDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectDatabase")
            .Options;

        using (var context = new ProjectClockDbContext(options))
        {
            context.Projects.Add(new Project
            {
                Name = "Existing Project",
                Organization = new Organization { Name = "Organization" }
            });
            await context.SaveChangesAsync();

            var projectServices = new ProjectServices(context, Substitute.For<IMapper>());

            var createProjectDto = new CreateProjectDto
            {
                ProjectName = "Existing Project",
                OrganizationName = "Organization"
            };

            // Act
            var result = await projectServices.Create(createProjectDto);

            // Assert
            result.Should().BeFalse();
        }
    }

    //Update

    [Fact]
    public async Task Update_ExistProject_ReturnsTrue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProjectClockDbContext>()
            .UseInMemoryDatabase(databaseName: "ProjectDatabase")
            .Options;

        using (var context = new ProjectClockDbContext(options))
        {
            var projectServices = new ProjectServices(context, Substitute.For<IMapper>());
            context.Projects.Add(new Project
            {
                Id = 1,
                Name = "Existing Project",
                Organization = new Organization { Name = "Organization" }
            });
            await context.SaveChangesAsync();

            var projectDto = new ProjectDto
            {
                Id = 1,
                Name = "Updated Project"
            };

            // Act
            await projectServices.Update(projectDto);

            // Assert
            var updatedProject = await context.Projects.FindAsync(projectDto.Id);
            updatedProject.Name.Should().Be(projectDto.Name);
        }
    }
}
