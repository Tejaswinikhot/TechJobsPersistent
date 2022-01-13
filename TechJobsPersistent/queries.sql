--Part 1

desc Jobs

--Part 2

select Name as Employers from techjobs.Employers where UPPER(location) = 'ST. LOUIS CITY'

--Part 3

SELECT distinct skills.Name, skills.description 
FROM techjobs.Skills skills
inner join techjobs.JobSkills jobSkills on skills.Id = jobSkills.skillid
order by skills.Name Asc;