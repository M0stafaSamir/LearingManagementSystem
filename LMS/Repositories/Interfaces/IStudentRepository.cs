﻿using LMS.Models;
using LMS.Models.InstractourModel;
using LMS.Models.StudentModels;
using Microsoft.EntityFrameworkCore;

public interface IStudentRepository
{
    // Course-related actions
    IEnumerable<Course> GetAllCourses(string search = "", string category = null);
    Course GetCourseDetails(int courseId);
    IEnumerable<Course> SearchCoursesByName(string name);
    IEnumerable<Course> SearchCoursesByCategory(Category category);
    IEnumerable<Course> GetRecommendedCourses(string studentId);
    IEnumerable<Category> GetAllCategories();

    // Certificate actions
    CertificateForStudent GetCertificateForCourse(string studentId, int courseId);
    bool CheckCourseCompletion(string studentId, int courseId);
    Task<IEnumerable<CertificateForStudent>> GetAllCertificates(string studentId);

    // Notes management
    void AddNoteToLesson(string studentId, int lessonId, string content);
    IEnumerable<Note> GetAllNotes(string studentId,int lessonId);
    void UpdateNote(int noteId, string newContent);
    void DeleteNote(int noteId);

    // Purchase management
    void PurchaseCourse(string studentId, int courseId);
    IEnumerable<Course> GetAllPurchases(string studentId);

    // Review & Rating management
    void AddReview(ReviewedCourse reviewedCourse);
    void UpdateReview(int reviewId, string newReviewText, int newRating);
    void DeleteReview(int reviewId);
    IEnumerable<ReviewedCourse> GetAllReviews(int courseId);

    // Enrollment management
    IEnumerable<StudentEnrollCourse> GetEnrolledCourses(string studentId);
    void EnrollInCourse(string studentId, int courseId);
    void RemoveEnrollment(string studentId, int courseId);
    bool HasPurchasedCourse(int courseId, string studentId);

    // Lesson progress tracking
    IEnumerable<Lesson> GetAllLessonsInCourse(int courseId);
    void MarkLessonAsStudied(string studentId, int lessonId);
    public Lesson GetLessonDetails(int lessonId, string studentId);

    double GetCourseProgress(string studentId, int courseId);

    // Wishlist management
    void AddToWishlist(string studentId, int courseId);
    IEnumerable<WishList> GetWishlist(string studentId);
    void RemoveFromWishlist(string studentId, int courseId);
}
