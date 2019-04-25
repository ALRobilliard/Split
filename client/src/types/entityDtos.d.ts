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

interface SplitPaymentDto {
  splitPaymentId: string,
  transactionId: string,
  payeeId: string,
  amount: number,
  createdOn: Date,
  modifiedOn: Date
}

interface TransactionDto {
  transactionId: string,
  categoryId?: string,
  transactionPartyId?: string,
  accountInId?: string,
  accountOutId?: string,
  amount?: number,
  isShared: boolean,
  transactionDate: Date,
  createdOn: Date,
  modifiedOn: Date,
  userId?: string
}

interface TransactionPartyDto {
  transactionPartyId: string,
  transactionPartyName: string,
  defaultCategoryId?: string,
  createdOn: Date,
  modifiedOn: Date,
  userId?: string
}

interface UserDto {
  id: string,
  username: string,
  firstName: string,
  lastName: string,
  token: string
}