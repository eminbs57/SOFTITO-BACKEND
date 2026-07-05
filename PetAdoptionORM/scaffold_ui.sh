#!/bin/bash
PROJECT_PATH="/Users/muhammeteminbas/Desktop/PetAdoptionORM/PetAdoptionORM"

# Create Admin Area structure
mkdir -p "$PROJECT_PATH/Areas/Admin/Controllers"
mkdir -p "$PROJECT_PATH/Areas/Admin/Views/Shared"
mkdir -p "$PROJECT_PATH/Areas/Admin/Views/Species"
mkdir -p "$PROJECT_PATH/Areas/Admin/Views/Breed"
mkdir -p "$PROJECT_PATH/Areas/Admin/Views/Pet"

# Create User Area structure
mkdir -p "$PROJECT_PATH/Areas/User/Controllers"
mkdir -p "$PROJECT_PATH/Areas/User/Views/Shared"
mkdir -p "$PROJECT_PATH/Areas/User/Views/AdoptionApplication"
mkdir -p "$PROJECT_PATH/Areas/User/Views/Donation"
mkdir -p "$PROJECT_PATH/Areas/User/Views/Home"

# Create wwwroot subfolders for image uploads
mkdir -p "$PROJECT_PATH/wwwroot/img/pets"

echo "UI Directories created."
