## Short summary
The scope requested for this home assignment was quite large, so I tried to keep it as simple as possible to be able to discuss it in the time box of the interview, so I intentionally ignored containerization, and CI/CD part of it, also I'll have the database in memory by leveraging from **EF core in-memory database**. for caching and pub/sub(Events) I decided to write comment of the potential places that it can be done. **I decided not write all the CRUD operation** such as operations for BC like Books, Batches, Inventories and instead manually insert them into the in-memory database. However, I tried to focus on the functionalities that has more complicity like Borrowing and returning with the possibility of reporting damaged or missing books on top of the functionalities requested in the assignment listed below:
1. **Inventory Insights:**
   1. What are the most borrowed books?
2. **User Activity:**
   1. Which users have borrowed the most books within a given time frame? 
   2. Estimate a user’s reading pace (pages per day) based on the borrow and return duration of a book, assuming continuous reading. 
3. **Borrowing Patterns:**
   1. What other books were borrowed by individuals who borrowed a specific book?

### General point of view:
1. **Member**: member is assumed to be an external BC, that's why we only keep a snapshot of the data only for reporting purposes. take that in mind that the identifier for member should naturally be a unique string in an average sized application preferably a sequential GUID and for large or global scales the ID generator has its own architecture and complexity. for the purposes of simplicity and readability of the result I considered it as int 64 as a matter of the fact I considered all Ids as int 64.
2. **Book:**
   1. **Author:** For simplicity it was considered as a text, but it can have many properties that contribute to the user taste of choosing books, such ass genre, language, etc.
   2. **Subject:** For simplicity it was considered as a text, but you can do a lot more with Subject, also the Subject can be nested, and it is a very complex topic.
3. **Batch:** A batch is an entity that holds data of a book with its unique ISBN, and the reason is each edition of a book has its own ISBN. In batch entity we are assuming that the library keeps batches of the book together of course it is wrong, but it's just for simplicity.
   1. **InventoryItem:** Upholds total, loaned, damaged, and missing quantity of a batch.
4. **Loan:** Hold data for the lent item and to whom it was loaned, also date related information such as Loan and due date.
   1. **LoanItem:** this entity keeps data related to the batch, book, state of the loaned item.
5. **LoanSetting:**
   1. Settings of a library application for a loaning is a lot more complex I just kept it simple to keep the scope in checked.
6. **Return Policy, Loyalty program, and potential fee for damaged or missing books:**
   1. I had these topics in mind but I had to let go of them to keep the scope in checked.