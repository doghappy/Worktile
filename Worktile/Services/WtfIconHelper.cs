﻿using System.Linq;
using Worktile.Models;

namespace Worktile.Services
{
    public static class WtfIconHelper
    {
        static WtfIconHelper()
        {
            Icons = new[]
            {
                new WtfIcon { Class = "wtf-plus", Glyph = "\ue634" },
                new WtfIcon { Class = "wtf-close", Glyph = "\ue6b3" },
                new WtfIcon { Class = "wtf-check-square", Glyph = "\ue69b" },
                new WtfIcon { Class = "wtf-dashboard-o", Glyph = "\ue6e6" },
                new WtfIcon { Class = "wtf-move", Glyph = "\ue6b4" },
                new WtfIcon { Class = "wtf-link-entity", Glyph = "\ue6e5" },
                new WtfIcon { Class = "wtf-due-date", Glyph = "\ue6d8" },
                new WtfIcon { Class = "wtf-begin-date", Glyph = "\ue6d9" },
                new WtfIcon { Class = "wtf-appraisal", Glyph = "\ue601" },
                new WtfIcon { Class = "wtf-approval-o", Glyph = "\ue602" },
                new WtfIcon { Class = "wtf-meeting-o", Glyph = "\ue603" },
                new WtfIcon { Class = "wtf-contacts", Glyph = "\ue604" },
                new WtfIcon { Class = "wtf-crm-o", Glyph = "\ue605" },
                new WtfIcon { Class = "wtf-portal-o", Glyph = "\ue606" },
                new WtfIcon { Class = "wtf-leave-o", Glyph = "\ue607" },
                new WtfIcon { Class = "wtf-leave", Glyph = "\ue608" },
                new WtfIcon { Class = "wtf-appraisal-o", Glyph = "\ue609" },
                new WtfIcon { Class = "wtf-okr", Glyph = "\ue60a" },
                new WtfIcon { Class = "wtf-bulletin-o", Glyph = "\ue60b" },
                new WtfIcon { Class = "wtf-report-o", Glyph = "\ue60c" },
                new WtfIcon { Class = "wtf-report", Glyph = "\ue60d" },
                new WtfIcon { Class = "wtf-bulletin", Glyph = "\ue60e" },
                new WtfIcon { Class = "wtf-crm", Glyph = "\ue60f" },
                new WtfIcon { Class = "wtf-portal", Glyph = "\ue610" },
                new WtfIcon { Class = "wtf-approval", Glyph = "\ue611" },
                new WtfIcon { Class = "wtf-meeting", Glyph = "\ue612" },
                new WtfIcon { Class = "wtf-okr-o", Glyph = "\ue613" },
                new WtfIcon { Class = "wtf-task-o", Glyph = "\ue614" },
                new WtfIcon { Class = "wtf-calendar", Glyph = "\ue615" },
                new WtfIcon { Class = "wtf-drive-o", Glyph = "\ue616" },
                new WtfIcon { Class = "wtf-contacts-o", Glyph = "\ue617" },
                new WtfIcon { Class = "wtf-message-o", Glyph = "\ue618" },
                new WtfIcon { Class = "wtf-calendar-o", Glyph = "\ue619" },
                new WtfIcon { Class = "wtf-task", Glyph = "\ue61a" },
                new WtfIcon { Class = "wtf-apps-o", Glyph = "\ue61b" },
                new WtfIcon { Class = "wtf-drive", Glyph = "\ue61c" },
                new WtfIcon { Class = "wtf-apps", Glyph = "\ue61d" },
                new WtfIcon { Class = "wtf-message", Glyph = "\ue61e" },
                new WtfIcon { Class = "wtf-portal-department-o", Glyph = "\ue61f" },
                new WtfIcon { Class = "wtf-portal-personal-o", Glyph = "\ue620" },
                new WtfIcon { Class = "wtf-portal-custom-o", Glyph = "\ue621" },
                new WtfIcon { Class = "wtf-portal-team-o", Glyph = "\ue622" },
                new WtfIcon { Class = "wtf-more", Glyph = "\ue623" },
                new WtfIcon { Class = "wtf-folder", Glyph = "\ue624" },
                new WtfIcon { Class = "wtf-folder-private", Glyph = "\ue625" },
                new WtfIcon { Class = "wtf-download", Glyph = "\ue626" },
                new WtfIcon { Class = "wtf-upload", Glyph = "\ue627" },
                new WtfIcon { Class = "wtf-full-screen", Glyph = "\ue629" },
                new WtfIcon { Class = "wtf-view-history", Glyph = "\ue62a" },
                new WtfIcon { Class = "wtf-download-20", Glyph = "\ue628" },
                new WtfIcon { Class = "wtf-heart", Glyph = "\ue62b" },
                new WtfIcon { Class = "wtf-edit-square-lg", Glyph = "\ue62f" },
                new WtfIcon { Class = "wtf-heart-o", Glyph = "\ue62c" },
                new WtfIcon { Class = "wtf-attachment-lg", Glyph = "\ue630" },
                new WtfIcon { Class = "wtf-send-square-lg", Glyph = "\ue631" },
                new WtfIcon { Class = "wtf-angle-left", Glyph = "\ue632" },
                new WtfIcon { Class = "wtf-list-ul", Glyph = "\ue63c" },
                new WtfIcon { Class = "wtf-circle-plus", Glyph = "\ue635" },
                new WtfIcon { Class = "wtf-arrow-up", Glyph = "\ue62d" },
                new WtfIcon { Class = "wtf-arrow-down", Glyph = "\ue62e" },
                new WtfIcon { Class = "wtf-commenting-o", Glyph = "\ue636" },
                new WtfIcon { Class = "wtf-search-o", Glyph = "\ue637" },
                new WtfIcon { Class = "wtf-th-plus", Glyph = "\ue638" },
                new WtfIcon { Class = "wtf-trash-o", Glyph = "\ue639" },
                new WtfIcon { Class = "wtf-label-lg-o", Glyph = "\ue63a" },
                new WtfIcon { Class = "wtf-angle-double-down", Glyph = "\ue63b" },
                new WtfIcon { Class = "wtf-th-large", Glyph = "\ue63d" },
                new WtfIcon { Class = "wtf-filter-line", Glyph = "\ue63e" },
                new WtfIcon { Class = "wtf-user-group-o", Glyph = "\ue63f" },
                new WtfIcon { Class = "wtf-setting-o", Glyph = "\ue640" },
                new WtfIcon { Class = "wtf-project-add-o", Glyph = "\ue600" },
                new WtfIcon { Class = "wtf-portal-add-o", Glyph = "\ue641" },
                new WtfIcon { Class = "wtf-user-o", Glyph = "\ue642" },
                new WtfIcon { Class = "wtf-grade-o", Glyph = "\ue643" },
                new WtfIcon { Class = "wtf-key-o", Glyph = "\ue644" },
                new WtfIcon { Class = "wtf-okr-sm-o", Glyph = "\ue645" },
                new WtfIcon { Class = "wtf-edit-o", Glyph = "\ue646" },
                new WtfIcon { Class = "wtf-user-group", Glyph = "\ue717" },
                new WtfIcon { Class = "wtf-contact-send", Glyph = "\ue718" },
                new WtfIcon { Class = "wtf-phone", Glyph = "\ue719" },
                new WtfIcon { Class = "wtf-organization", Glyph = "\ue71a" },
                new WtfIcon { Class = "wtf-email", Glyph = "\ue71b" },
                new WtfIcon { Class = "wtf-title", Glyph = "\ue71c" },
                new WtfIcon { Class = "wtf-star-o", Glyph = "\ue71d" },
                new WtfIcon { Class = "wtf-star", Glyph = "\ue71e" },
                new WtfIcon { Class = "wtf-add-member", Glyph = "\ue71f" },
                new WtfIcon { Class = "wtf-message-add", Glyph = "\ue647" },
                new WtfIcon { Class = "wtf-task-add", Glyph = "\ue648" },
                new WtfIcon { Class = "wtf-calendar-add", Glyph = "\ue649" },
                new WtfIcon { Class = "wtf-drive-upload", Glyph = "\ue64a" },
                new WtfIcon { Class = "wtf-contract-o", Glyph = "\ue64b" },
                new WtfIcon { Class = "wtf-contract-status-o", Glyph = "\ue64c" },
                new WtfIcon { Class = "wtf-contract-lg-o", Glyph = "\ue64d" },
                new WtfIcon { Class = "wtf-billing-o", Glyph = "\ue64e" },
                new WtfIcon { Class = "wtf-payment-o", Glyph = "\ue64f" },
                new WtfIcon { Class = "wtf-customer-o", Glyph = "\ue650" },
                new WtfIcon { Class = "wtf-calendar-check-o", Glyph = "\ue651" },
                new WtfIcon { Class = "wtf-info-o", Glyph = "\ue652" },
                new WtfIcon { Class = "wtf-level-high", Glyph = "\ue720" },
                new WtfIcon { Class = "wtf-level-low", Glyph = "\ue721" },
                new WtfIcon { Class = "wtf-level-secondary", Glyph = "\ue722" },
                new WtfIcon { Class = "wtf-calendar-add-o", Glyph = "\ue653" },
                new WtfIcon { Class = "wtf-message-add-o", Glyph = "\ue633" },
                new WtfIcon { Class = "wtf-link", Glyph = "\ue654" },
                new WtfIcon { Class = "wtf-empty-o", Glyph = "\ue655" },
                new WtfIcon { Class = "wtf-business-card", Glyph = "\ue656" },
                new WtfIcon { Class = "wtf-weather", Glyph = "\ue657" },
                new WtfIcon { Class = "wtf-bar-chart-increment", Glyph = "\ue659" },
                new WtfIcon { Class = "wtf-clock", Glyph = "\ue65a" },
                new WtfIcon { Class = "wtf-th-large-app", Glyph = "\ue65b" },
                new WtfIcon { Class = "wtf-rss", Glyph = "\ue65c" },
                new WtfIcon { Class = "wtf-qr-code", Glyph = "\ue65d" },
                new WtfIcon { Class = "wtf-file-text", Glyph = "\ue65f" },
                new WtfIcon { Class = "wtf-bars-percent", Glyph = "\ue660" },
                new WtfIcon { Class = "wtf-text", Glyph = "\ue658" },
                new WtfIcon { Class = "wtf-image", Glyph = "\ue65e" },
                new WtfIcon { Class = "wtf-calendar-date", Glyph = "\ue661" },
                new WtfIcon { Class = "wtf-user-upcoming", Glyph = "\ue662" },
                new WtfIcon { Class = "wtf-trophy", Glyph = "\ue663" },
                new WtfIcon { Class = "wtf-inbox", Glyph = "\ue665" },
                new WtfIcon { Class = "wtf-archive", Glyph = "\ue664" },
                new WtfIcon { Class = "wtf-inbox-o", Glyph = "\ue666" },
                new WtfIcon { Class = "wtf-project-private", Glyph = "\ue667" },
                new WtfIcon { Class = "wtf-project-private-o", Glyph = "\ue668" },
                new WtfIcon { Class = "wtf-submit-approval", Glyph = "\ue669" },
                new WtfIcon { Class = "wtf-attachment", Glyph = "\ue66f" },
                new WtfIcon { Class = "wtf-image-o", Glyph = "\ue66a" },
                new WtfIcon { Class = "wtf-task-sub-lg-o", Glyph = "\ue66b" },
                new WtfIcon { Class = "wtf-task-sub-o", Glyph = "\ue66c" },
                new WtfIcon { Class = "wtf-more-lg", Glyph = "\ue66d" },
                new WtfIcon { Class = "wtf-label-o", Glyph = "\ue66e" },
                new WtfIcon { Class = "wtf-send-square", Glyph = "\ue670" },
                new WtfIcon { Class = "wtf-times-lg", Glyph = "\ue671" },
                new WtfIcon { Class = "wtf-label", Glyph = "\ue672" },
                new WtfIcon { Class = "wtf-progress-o", Glyph = "\ue673" },
                new WtfIcon { Class = "wtf-date-end", Glyph = "\ue674" },
                new WtfIcon { Class = "wtf-date-begin", Glyph = "\ue675" },
                new WtfIcon { Class = "wtf-times", Glyph = "\ue676" },
                new WtfIcon { Class = "wtf-home", Glyph = "\ue677" },
                new WtfIcon { Class = "wtf-map-marker", Glyph = "\ue678" },
                new WtfIcon { Class = "wtf-description", Glyph = "\ue679" },
                new WtfIcon { Class = "wtf-approval-process", Glyph = "\ue67e" },
                new WtfIcon { Class = "wtf-approval-detail", Glyph = "\ue67a" },
                new WtfIcon { Class = "wtf-bell-lg-o", Glyph = "\ue67b" },
                new WtfIcon { Class = "wtf-data-error", Glyph = "\ue723" },
                new WtfIcon { Class = "wtf-edit-th-o", Glyph = "\ue67c" },
                new WtfIcon { Class = "wtf-tablet", Glyph = "\ue67d" },
                new WtfIcon { Class = "wtf-approval-pending-o", Glyph = "\ue67f" },
                new WtfIcon { Class = "wtf-preview", Glyph = "\ue680" },
                new WtfIcon { Class = "wtf-dump", Glyph = "\ue681" },
                new WtfIcon { Class = "wtf-crm-contact", Glyph = "\ue682" },
                new WtfIcon { Class = "wtf-download-14", Glyph = "\ue683" },
                new WtfIcon { Class = "wtf-link-approval", Glyph = "\ue685" },
                new WtfIcon { Class = "wtf-link-file", Glyph = "\ue687" },
                new WtfIcon { Class = "wtf-link-event", Glyph = "\ue688" },
                new WtfIcon { Class = "wtf-link-report", Glyph = "\ue689" },
                new WtfIcon { Class = "wtf-contracts", Glyph = "\ue68a" },
                new WtfIcon { Class = "wtf-chart-pie", Glyph = "\ue68b" },
                new WtfIcon { Class = "wtf-chart-rag", Glyph = "\ue68c" },
                new WtfIcon { Class = "wtf-okr-progress", Glyph = "\ue68d" },
                new WtfIcon { Class = "wtf-chart-bar", Glyph = "\ue68e" },
                new WtfIcon { Class = "wtf-chart-digit", Glyph = "\ue68f" },
                new WtfIcon { Class = "wtf-customer", Glyph = "\ue690" },
                new WtfIcon { Class = "wtf-complete", Glyph = "\ue691" },
                new WtfIcon { Class = "wtf-drag", Glyph = "\ue692" },
                new WtfIcon { Class = "wtf-offline-o", Glyph = "\ue693" },
                new WtfIcon { Class = "wtf-create-task", Glyph = "\ue694" },
                new WtfIcon { Class = "wtf-create-message", Glyph = "\ue695" },
                new WtfIcon { Class = "wtf-create-calendar", Glyph = "\ue696" },
                new WtfIcon { Class = "wtf-upload-file", Glyph = "\ue697" },
                new WtfIcon { Class = "wtf-status-o", Glyph = "\ue698" },
                new WtfIcon { Class = "wtf-participant-o", Glyph = "\ue699" },
                new WtfIcon { Class = "wtf-info", Glyph = "\ue724" },
                new WtfIcon { Class = "wtf-task-lg", Glyph = "\ue686" },
                new WtfIcon { Class = "wtf-check-square-o", Glyph = "\ue69a" },
                new WtfIcon { Class = "wtf-circle-plus-o", Glyph = "\ue69c" },
                new WtfIcon { Class = "wtf-expand", Glyph = "\ue69d" },
                new WtfIcon { Class = "wtf-collapse", Glyph = "\ue69e" },
                new WtfIcon { Class = "wtf-reset", Glyph = "\ue69f" },
                new WtfIcon { Class = "wtf-reduce", Glyph = "\ue6a0" },
                new WtfIcon { Class = "wtf-enlarge", Glyph = "\ue6a1" },
                new WtfIcon { Class = "wtf-organization-o", Glyph = "\ue6a2" },
                new WtfIcon { Class = "wtf-draft", Glyph = "\ue6a3" },
                new WtfIcon { Class = "wtf-maybe", Glyph = "\ue6a4" },
                new WtfIcon { Class = "wtf-accepted", Glyph = "\ue6a5" },
                new WtfIcon { Class = "wtf-maybe-c", Glyph = "\ue6a6" },
                new WtfIcon { Class = "wtf-accepted-c", Glyph = "\ue6a7" },
                new WtfIcon { Class = "wtf-contract-add-o", Glyph = "\ue6a8" },
                new WtfIcon { Class = "wtf-eye", Glyph = "\ue6a9" },
                new WtfIcon { Class = "wtf-eye-slash", Glyph = "\ue6aa" },
                new WtfIcon { Class = "wtf-processing", Glyph = "\ue6ab" },
                new WtfIcon { Class = "wtf-completed", Glyph = "\ue6ac" },
                new WtfIcon { Class = "wtf-pending", Glyph = "\ue6ad" },
                new WtfIcon { Class = "wtf-type-ios", Glyph = "\ue725" },
                new WtfIcon { Class = "wtf-type-worksheet", Glyph = "\ue726" },
                new WtfIcon { Class = "wtf-type-task", Glyph = "\ue727" },
                new WtfIcon { Class = "wtf-type-man", Glyph = "\ue728" },
                new WtfIcon { Class = "wtf-type-pending", Glyph = "\ue729" },
                new WtfIcon { Class = "wtf-type-message", Glyph = "\ue72a" },
                new WtfIcon { Class = "wtf-type-pc", Glyph = "\ue72b" },
                new WtfIcon { Class = "wtf-type-star", Glyph = "\ue72c" },
                new WtfIcon { Class = "wtf-type-money", Glyph = "\ue72d" },
                new WtfIcon { Class = "wtf-type-bug", Glyph = "\ue72e" },
                new WtfIcon { Class = "wtf-type-demand", Glyph = "\ue72f" },
                new WtfIcon { Class = "wtf-type-android", Glyph = "\ue730" },
                new WtfIcon { Class = "wtf-template-task", Glyph = "\ue731" },
                new WtfIcon { Class = "wtf-template-scrum", Glyph = "\ue732" },
                new WtfIcon { Class = "wtf-template-personal", Glyph = "\ue733" },
                new WtfIcon { Class = "wtf-template-kanban", Glyph = "\ue734" },
                new WtfIcon { Class = "wtf-template-bug", Glyph = "\ue735" },
                new WtfIcon { Class = "wtf-template-administration", Glyph = "\ue736" },
                new WtfIcon { Class = "wtf-template-crm", Glyph = "\ue737" },
                new WtfIcon { Class = "wtf-template-project", Glyph = "\ue738" },
                new WtfIcon { Class = "wtf-step-division", Glyph = "\ue6ae" },
                new WtfIcon { Class = "wtf-copy", Glyph = "\ue6b0" },
                new WtfIcon { Class = "wtf-examine", Glyph = "\ue6af" },
                new WtfIcon { Class = "wtf-angle-right", Glyph = "\ue739" },
                new WtfIcon { Class = "wtf-schedule-o", Glyph = "\ue6b1" },
                new WtfIcon { Class = "wtf-schedule", Glyph = "\ue6b2" },
                new WtfIcon { Class = "wtf-dot-circle-o", Glyph = "\ue6b5" },
                new WtfIcon { Class = "wtf-dot-circle", Glyph = "\ue6b6" },
                new WtfIcon { Class = "wtf-angle-down-blod", Glyph = "\ue6b7" },
                new WtfIcon { Class = "wtf-angle-down", Glyph = "\ue6b8" },
                new WtfIcon { Class = "wtf-angle-up", Glyph = "\ue73a" },
                new WtfIcon { Class = "wtf-table", Glyph = "\ue6b9" },
                new WtfIcon { Class = "wtf-kanban", Glyph = "\ue6ba" },
                new WtfIcon { Class = "wtf-list", Glyph = "\ue6bb" },
                new WtfIcon { Class = "wtf-list-o", Glyph = "\ue6bc" },
                new WtfIcon { Class = "wtf-kanban-o", Glyph = "\ue6bd" },
                new WtfIcon { Class = "wtf-table-o", Glyph = "\ue6be" },
                new WtfIcon { Class = "wtf-timeline-o", Glyph = "\ue6bf" },
                new WtfIcon { Class = "wtf-insight-o", Glyph = "\ue6c0" },
                new WtfIcon { Class = "wtf-iteration-o", Glyph = "\ue6c1" },
                new WtfIcon { Class = "wtf-calendar-view-o", Glyph = "\ue6c2" },
                new WtfIcon { Class = "wtf-iteration", Glyph = "\ue6c3" },
                new WtfIcon { Class = "wtf-insight", Glyph = "\ue6c4" },
                new WtfIcon { Class = "wtf-calendar-view", Glyph = "\ue6c5" },
                new WtfIcon { Class = "wtf-timeline", Glyph = "\ue6c6" },
                new WtfIcon { Class = "wtf-workload", Glyph = "\ue6c7" },
                new WtfIcon { Class = "wtf-pause-circle", Glyph = "\ue6c8" },
                new WtfIcon { Class = "wtf-completed-circle", Glyph = "\ue6c9" },
                new WtfIcon { Class = "wtf-lock", Glyph = "\ue6ca" },
                new WtfIcon { Class = "wtf-unlock", Glyph = "\ue6cb" },
                new WtfIcon { Class = "wtf-times-circle", Glyph = "\ue6cc" },
                new WtfIcon { Class = "wtf-columns", Glyph = "\ue6cd" },
                new WtfIcon { Class = "wtf-exchange-alt", Glyph = "\ue6ce" },
                new WtfIcon { Class = "wtf-bold", Glyph = "\ue6cf" },
                new WtfIcon { Class = "wtf-check-square1", Glyph = "\ue6d0" },
                new WtfIcon { Class = "wtf-code", Glyph = "\ue6d1" },
                new WtfIcon { Class = "wtf-heading", Glyph = "\ue6d2" },
                new WtfIcon { Class = "wtf-image1", Glyph = "\ue6d3" },
                new WtfIcon { Class = "wtf-italic", Glyph = "\ue6d4" },
                new WtfIcon { Class = "wtf-link1", Glyph = "\ue6d5" },
                new WtfIcon { Class = "wtf-list-ul1", Glyph = "\ue6d6" },
                new WtfIcon { Class = "wtf-long-arrow-alt-right", Glyph = "\ue6d7" },
                new WtfIcon { Class = "wtf-list-ol", Glyph = "\ue6da" },
                new WtfIcon { Class = "wtf-minus", Glyph = "\ue6db" },
                new WtfIcon { Class = "wtf-quote-left", Glyph = "\ue6dc" },
                new WtfIcon { Class = "wtf-sliders-h", Glyph = "\ue6dd" },
                new WtfIcon { Class = "wtf-smile", Glyph = "\ue6de" },
                new WtfIcon { Class = "wtf-square", Glyph = "\ue6df" },
                new WtfIcon { Class = "wtf-strikethrough", Glyph = "\ue6e0" },
                new WtfIcon { Class = "wtf-superscript", Glyph = "\ue6e1" },
                new WtfIcon { Class = "wtf-table-toolbar", Glyph = "\ue6e2" },
                new WtfIcon { Class = "wtf-underline", Glyph = "\ue6e3" },
                new WtfIcon { Class = "wtf-expand-arrows-alt", Glyph = "\ue6e4" },
                new WtfIcon { Class = "wtf-dashboard-circle", Glyph = "\ue6ea" },
                new WtfIcon { Class = "wtf-iphone", Glyph = "\ue6e7" },
                new WtfIcon { Class = "wtf-pc", Glyph = "\ue6e8" },
                new WtfIcon { Class = "wtf-android", Glyph = "\ue6e9" },
                new WtfIcon { Class = "wtf-unselected-o", Glyph = "\ue6eb" },
                new WtfIcon { Class = "wtf-property-attachment", Glyph = "\ue6ec" },
                new WtfIcon { Class = "wtf-property-tag", Glyph = "\ue6ed" },
                new WtfIcon { Class = "wtf-property-priority", Glyph = "\ue6ee" },
                new WtfIcon { Class = "wtf-property-iteration", Glyph = "\ue6ef" },
                new WtfIcon { Class = "wtf-property-sub-task", Glyph = "\ue6f0" },
                new WtfIcon { Class = "wtf-property-number", Glyph = "\ue6f1" },
                new WtfIcon { Class = "wtf-property-text", Glyph = "\ue6f2" },
                new WtfIcon { Class = "wtf-property-textarea", Glyph = "\ue6f3" },
                new WtfIcon { Class = "wtf-property-multi-select", Glyph = "\ue6f4" },
                new WtfIcon { Class = "wtf-relation-o", Glyph = "\ue684" },
                new WtfIcon { Class = "wtf-pending-lg-o", Glyph = "\ue6f5" },
                new WtfIcon { Class = "wtf-completed-lg-o", Glyph = "\ue6f6" },
                new WtfIcon { Class = "wtf-relation-th-o", Glyph = "\ue6fa" },
                new WtfIcon { Class = "wtf-workload-o", Glyph = "\ue6fb" },
                new WtfIcon { Class = "wtf-workload-th-o", Glyph = "\ue6fc" },
                new WtfIcon { Class = "wtf-begin-date-th-o", Glyph = "\ue6fd" },
                new WtfIcon { Class = "wtf-processing-lg-o", Glyph = "\ue6f7" },
                new WtfIcon { Class = "wtf-due-date-th-o", Glyph = "\ue6fe" },
                new WtfIcon { Class = "wtf-user-add-o", Glyph = "\ue6f8" },
                new WtfIcon { Class = "wtf-user-add-th-o", Glyph = "\ue6f9" },
                new WtfIcon { Class = "wtf-begin-date-o", Glyph = "\ue6ff" },
                new WtfIcon { Class = "wtf-due-date-o", Glyph = "\ue700" },
                new WtfIcon { Class = "wtf-property-select", Glyph = "\ue702" },
                new WtfIcon { Class = "wtf-property-date", Glyph = "\ue703" },
                new WtfIcon { Class = "wtf-property-workload", Glyph = "\ue704" },
                new WtfIcon { Class = "wtf-property-members", Glyph = "\ue705" },
                new WtfIcon { Class = "wtf-property-member", Glyph = "\ue706" },
                new WtfIcon { Class = "wtf-workload-analytic", Glyph = "\ue701" },
                new WtfIcon { Class = "wtf-time-view", Glyph = "\ue707" },
                new WtfIcon { Class = "wtf-analytic-insight", Glyph = "\ue708" },
                new WtfIcon { Class = "wtf-my", Glyph = "\ue709" },
                new WtfIcon { Class = "wtf-subordinate", Glyph = "\ue70a" },
                new WtfIcon { Class = "wtf-project-setting", Glyph = "\ue73b" },
                new WtfIcon { Class = "wtf-link-leave", Glyph = "\ue70b" },
                new WtfIcon { Class = "wtf-mission", Glyph = "\ue70c" },
                new WtfIcon { Class = "wtf-mission-o", Glyph = "\ue70d" },
                new WtfIcon { Class = "wtf-connect", Glyph = "\ue70e" },
                new WtfIcon { Class = "wtf-disconnect", Glyph = "\ue70f" }
            };
        }

        public static WtfIcon[] Icons { get; }

        public static string GetGlyph(string @class)
        {
            return Icons.Single(i => i.Class == @class).Glyph;
        }
    }
}