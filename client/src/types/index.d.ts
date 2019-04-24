interface AccountDto {
  accountId: string,
  accountName: string,
  createdOn: Date,
  modifiedOn: Date
}

interface CategoryDto {
  categoryId: string,
  categoryName: string,
  createdOn: Date,
  modifiedOn: Date,
  userId: string,
  categoryType: number
}

interface UserDto {
  id: string,
  username: string,
  firstName: string,
  lastName: string,
  token: string
}