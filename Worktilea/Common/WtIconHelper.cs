﻿using System.Linq;
using Worktile.Enums;
using Worktile.Models;

namespace Worktile.Common
{
    public static class WtIconHelper
    {
        static WtIconHelper()
        {
            Icons = new[]
            {
                new WtIcon { Class = "wtf-plus", Glyph = "\ue634" },
                new WtIcon { Class = "wtf-close", Glyph = "\ue6b3" },
                new WtIcon { Class = "wtf-check-square", Glyph = "\ue69b" },
                new WtIcon { Class = "wtf-dashboard-o", Glyph = "\ue6e6" },
                new WtIcon { Class = "wtf-move", Glyph = "\ue6b4" },
                new WtIcon { Class = "wtf-link-entity", Glyph = "\ue6e5" },
                new WtIcon { Class = "wtf-due-date", Glyph = "\ue6d8" },
                new WtIcon { Class = "wtf-begin-date", Glyph = "\ue6d9" },
                new WtIcon { Class = "wtf-appraisal", Glyph = "\ue601" },
                new WtIcon { Class = "wtf-approval-o", Glyph = "\ue602" },
                new WtIcon { Class = "wtf-meeting-o", Glyph = "\ue603" },
                new WtIcon { Class = "wtf-contacts", Glyph = "\ue604" },
                new WtIcon { Class = "wtf-crm-o", Glyph = "\ue605" },
                new WtIcon { Class = "wtf-portal-o", Glyph = "\ue606" },
                new WtIcon { Class = "wtf-leave-o", Glyph = "\ue607" },
                new WtIcon { Class = "wtf-leave", Glyph = "\ue608" },
                new WtIcon { Class = "wtf-appraisal-o", Glyph = "\ue609" },
                new WtIcon { Class = "wtf-okr", Glyph = "\ue60a" },
                new WtIcon { Class = "wtf-bulletin-o", Glyph = "\ue60b" },
                new WtIcon { Class = "wtf-report-o", Glyph = "\ue60c" },
                new WtIcon { Class = "wtf-report", Glyph = "\ue60d" },
                new WtIcon { Class = "wtf-bulletin", Glyph = "\ue60e" },
                new WtIcon { Class = "wtf-crm", Glyph = "\ue60f" },
                new WtIcon { Class = "wtf-portal", Glyph = "\ue610" },
                new WtIcon { Class = "wtf-approval", Glyph = "\ue611" },
                new WtIcon { Class = "wtf-meeting", Glyph = "\ue612" },
                new WtIcon { Class = "wtf-okr-o", Glyph = "\ue613" },
                new WtIcon { Class = "wtf-task-o", Glyph = "\ue614" },
                new WtIcon { Class = "wtf-calendar", Glyph = "\ue615" },
                new WtIcon { Class = "wtf-drive-o", Glyph = "\ue616" },
                new WtIcon { Class = "wtf-contacts-o", Glyph = "\ue617" },
                new WtIcon { Class = "wtf-message-o", Glyph = "\ue618" },
                new WtIcon { Class = "wtf-calendar-o", Glyph = "\ue619" },
                new WtIcon { Class = "wtf-task", Glyph = "\ue61a" },
                new WtIcon { Class = "wtf-apps-o", Glyph = "\ue61b" },
                new WtIcon { Class = "wtf-drive", Glyph = "\ue61c" },
                new WtIcon { Class = "wtf-apps", Glyph = "\ue61d" },
                new WtIcon { Class = "wtf-message", Glyph = "\ue61e" },
                new WtIcon { Class = "wtf-portal-department-o", Glyph = "\ue61f" },
                new WtIcon { Class = "wtf-portal-personal-o", Glyph = "\ue620" },
                new WtIcon { Class = "wtf-portal-custom-o", Glyph = "\ue621" },
                new WtIcon { Class = "wtf-portal-team-o", Glyph = "\ue622" },
                new WtIcon { Class = "wtf-more", Glyph = "\ue623" },
                new WtIcon { Class = "wtf-folder", Glyph = "\ue624" },
                new WtIcon { Class = "wtf-folder-private", Glyph = "\ue625" },
                new WtIcon { Class = "wtf-download", Glyph = "\ue626" },
                new WtIcon { Class = "wtf-upload", Glyph = "\ue627" },
                new WtIcon { Class = "wtf-full-screen", Glyph = "\ue629" },
                new WtIcon { Class = "wtf-view-history", Glyph = "\ue62a" },
                new WtIcon { Class = "wtf-download-20", Glyph = "\ue628" },
                new WtIcon { Class = "wtf-heart", Glyph = "\ue62b" },
                new WtIcon { Class = "wtf-edit-square-lg", Glyph = "\ue62f" },
                new WtIcon { Class = "wtf-heart-o", Glyph = "\ue62c" },
                new WtIcon { Class = "wtf-attachment-lg", Glyph = "\ue630" },
                new WtIcon { Class = "wtf-send-square-lg", Glyph = "\ue631" },
                new WtIcon { Class = "wtf-angle-left", Glyph = "\ue632" },
                new WtIcon { Class = "wtf-list-ul", Glyph = "\ue63c" },
                new WtIcon { Class = "wtf-circle-plus", Glyph = "\ue635" },
                new WtIcon { Class = "wtf-arrow-up", Glyph = "\ue62d" },
                new WtIcon { Class = "wtf-arrow-down", Glyph = "\ue62e" },
                new WtIcon { Class = "wtf-commenting-o", Glyph = "\ue636" },
                new WtIcon { Class = "wtf-search-o", Glyph = "\ue637" },
                new WtIcon { Class = "wtf-th-plus", Glyph = "\ue638" },
                new WtIcon { Class = "wtf-trash-o", Glyph = "\ue639" },
                new WtIcon { Class = "wtf-label-lg-o", Glyph = "\ue63a" },
                new WtIcon { Class = "wtf-angle-double-down", Glyph = "\ue63b" },
                new WtIcon { Class = "wtf-th-large", Glyph = "\ue63d" },
                new WtIcon { Class = "wtf-filter-line", Glyph = "\ue63e" },
                new WtIcon { Class = "wtf-user-group-o", Glyph = "\ue63f" },
                new WtIcon { Class = "wtf-setting-o", Glyph = "\ue640" },
                new WtIcon { Class = "wtf-project-add-o", Glyph = "\ue600" },
                new WtIcon { Class = "wtf-portal-add-o", Glyph = "\ue641" },
                new WtIcon { Class = "wtf-user-o", Glyph = "\ue642" },
                new WtIcon { Class = "wtf-grade-o", Glyph = "\ue643" },
                new WtIcon { Class = "wtf-key-o", Glyph = "\ue644" },
                new WtIcon { Class = "wtf-okr-sm-o", Glyph = "\ue645" },
                new WtIcon { Class = "wtf-edit-o", Glyph = "\ue646" },
                new WtIcon { Class = "wtf-user-group", Glyph = "\ue717" },
                new WtIcon { Class = "wtf-contact-send", Glyph = "\ue718" },
                new WtIcon { Class = "wtf-phone", Glyph = "\ue719" },
                new WtIcon { Class = "wtf-organization", Glyph = "\ue71a" },
                new WtIcon { Class = "wtf-email", Glyph = "\ue71b" },
                new WtIcon { Class = "wtf-title", Glyph = "\ue71c" },
                new WtIcon { Class = "wtf-star-o", Glyph = "\ue71d" },
                new WtIcon { Class = "wtf-star", Glyph = "\ue71e" },
                new WtIcon { Class = "wtf-add-member", Glyph = "\ue71f" },
                new WtIcon { Class = "wtf-message-add", Glyph = "\ue647" },
                new WtIcon { Class = "wtf-task-add", Glyph = "\ue648" },
                new WtIcon { Class = "wtf-calendar-add", Glyph = "\ue649" },
                new WtIcon { Class = "wtf-drive-upload", Glyph = "\ue64a" },
                new WtIcon { Class = "wtf-contract-o", Glyph = "\ue64b" },
                new WtIcon { Class = "wtf-contract-status-o", Glyph = "\ue64c" },
                new WtIcon { Class = "wtf-contract-lg-o", Glyph = "\ue64d" },
                new WtIcon { Class = "wtf-billing-o", Glyph = "\ue64e" },
                new WtIcon { Class = "wtf-payment-o", Glyph = "\ue64f" },
                new WtIcon { Class = "wtf-customer-o", Glyph = "\ue650" },
                new WtIcon { Class = "wtf-calendar-check-o", Glyph = "\ue651" },
                new WtIcon { Class = "wtf-info-o", Glyph = "\ue652" },
                new WtIcon { Class = "wtf-level-high", Glyph = "\ue720" },
                new WtIcon { Class = "wtf-level-low", Glyph = "\ue721" },
                new WtIcon { Class = "wtf-level-secondary", Glyph = "\ue722" },
                new WtIcon { Class = "wtf-calendar-add-o", Glyph = "\ue653" },
                new WtIcon { Class = "wtf-message-add-o", Glyph = "\ue633" },
                new WtIcon { Class = "wtf-link", Glyph = "\ue654" },
                new WtIcon { Class = "wtf-empty-o", Glyph = "\ue655" },
                new WtIcon { Class = "wtf-business-card", Glyph = "\ue656" },
                new WtIcon { Class = "wtf-weather", Glyph = "\ue657" },
                new WtIcon { Class = "wtf-bar-chart-increment", Glyph = "\ue659" },
                new WtIcon { Class = "wtf-clock", Glyph = "\ue65a" },
                new WtIcon { Class = "wtf-th-large-app", Glyph = "\ue65b" },
                new WtIcon { Class = "wtf-rss", Glyph = "\ue65c" },
                new WtIcon { Class = "wtf-qr-code", Glyph = "\ue65d" },
                new WtIcon { Class = "wtf-file-text", Glyph = "\ue65f" },
                new WtIcon { Class = "wtf-bars-percent", Glyph = "\ue660" },
                new WtIcon { Class = "wtf-text", Glyph = "\ue658" },
                new WtIcon { Class = "wtf-image", Glyph = "\ue65e" },
                new WtIcon { Class = "wtf-calendar-date", Glyph = "\ue661" },
                new WtIcon { Class = "wtf-user-upcoming", Glyph = "\ue662" },
                new WtIcon { Class = "wtf-trophy", Glyph = "\ue663" },
                new WtIcon { Class = "wtf-inbox", Glyph = "\ue665" },
                new WtIcon { Class = "wtf-archive", Glyph = "\ue664" },
                new WtIcon { Class = "wtf-inbox-o", Glyph = "\ue666" },
                new WtIcon { Class = "wtf-project-private", Glyph = "\ue667" },
                new WtIcon { Class = "wtf-project-private-o", Glyph = "\ue668" },
                new WtIcon { Class = "wtf-submit-approval", Glyph = "\ue669" },
                new WtIcon { Class = "wtf-attachment", Glyph = "\ue66f" },
                new WtIcon { Class = "wtf-image-o", Glyph = "\ue66a" },
                new WtIcon { Class = "wtf-task-sub-lg-o", Glyph = "\ue66b" },
                new WtIcon { Class = "wtf-task-sub-o", Glyph = "\ue66c" },
                new WtIcon { Class = "wtf-more-lg", Glyph = "\ue66d" },
                new WtIcon { Class = "wtf-label-o", Glyph = "\ue66e" },
                new WtIcon { Class = "wtf-send-square", Glyph = "\ue670" },
                new WtIcon { Class = "wtf-times-lg", Glyph = "\ue671" },
                new WtIcon { Class = "wtf-label", Glyph = "\ue672" },
                new WtIcon { Class = "wtf-progress-o", Glyph = "\ue673" },
                new WtIcon { Class = "wtf-date-end", Glyph = "\ue674" },
                new WtIcon { Class = "wtf-date-begin", Glyph = "\ue675" },
                new WtIcon { Class = "wtf-times", Glyph = "\ue676" },
                new WtIcon { Class = "wtf-home", Glyph = "\ue677" },
                new WtIcon { Class = "wtf-map-marker", Glyph = "\ue678" },
                new WtIcon { Class = "wtf-description", Glyph = "\ue679" },
                new WtIcon { Class = "wtf-approval-process", Glyph = "\ue67e" },
                new WtIcon { Class = "wtf-approval-detail", Glyph = "\ue67a" },
                new WtIcon { Class = "wtf-bell-lg-o", Glyph = "\ue67b" },
                new WtIcon { Class = "wtf-data-error", Glyph = "\ue723" },
                new WtIcon { Class = "wtf-edit-th-o", Glyph = "\ue67c" },
                new WtIcon { Class = "wtf-tablet", Glyph = "\ue67d" },
                new WtIcon { Class = "wtf-approval-pending-o", Glyph = "\ue67f" },
                new WtIcon { Class = "wtf-preview", Glyph = "\ue680" },
                new WtIcon { Class = "wtf-dump", Glyph = "\ue681" },
                new WtIcon { Class = "wtf-crm-contact", Glyph = "\ue682" },
                new WtIcon { Class = "wtf-download-14", Glyph = "\ue683" },
                new WtIcon { Class = "wtf-link-approval", Glyph = "\ue685" },
                new WtIcon { Class = "wtf-link-file", Glyph = "\ue687" },
                new WtIcon { Class = "wtf-link-event", Glyph = "\ue688" },
                new WtIcon { Class = "wtf-link-report", Glyph = "\ue689" },
                new WtIcon { Class = "wtf-contracts", Glyph = "\ue68a" },
                new WtIcon { Class = "wtf-chart-pie", Glyph = "\ue68b" },
                new WtIcon { Class = "wtf-chart-rag", Glyph = "\ue68c" },
                new WtIcon { Class = "wtf-okr-progress", Glyph = "\ue68d" },
                new WtIcon { Class = "wtf-chart-bar", Glyph = "\ue68e" },
                new WtIcon { Class = "wtf-chart-digit", Glyph = "\ue68f" },
                new WtIcon { Class = "wtf-customer", Glyph = "\ue690" },
                new WtIcon { Class = "wtf-complete", Glyph = "\ue691" },
                new WtIcon { Class = "wtf-drag", Glyph = "\ue692" },
                new WtIcon { Class = "wtf-offline-o", Glyph = "\ue693" },
                new WtIcon { Class = "wtf-create-task", Glyph = "\ue694" },
                new WtIcon { Class = "wtf-create-message", Glyph = "\ue695" },
                new WtIcon { Class = "wtf-create-calendar", Glyph = "\ue696" },
                new WtIcon { Class = "wtf-upload-file", Glyph = "\ue697" },
                new WtIcon { Class = "wtf-status-o", Glyph = "\ue698" },
                new WtIcon { Class = "wtf-participant-o", Glyph = "\ue699" },
                new WtIcon { Class = "wtf-info", Glyph = "\ue724" },
                new WtIcon { Class = "wtf-task-lg", Glyph = "\ue686" },
                new WtIcon { Class = "wtf-check-square-o", Glyph = "\ue69a" },
                new WtIcon { Class = "wtf-circle-plus-o", Glyph = "\ue69c" },
                new WtIcon { Class = "wtf-expand", Glyph = "\ue69d" },
                new WtIcon { Class = "wtf-collapse", Glyph = "\ue69e" },
                new WtIcon { Class = "wtf-reset", Glyph = "\ue69f" },
                new WtIcon { Class = "wtf-reduce", Glyph = "\ue6a0" },
                new WtIcon { Class = "wtf-enlarge", Glyph = "\ue6a1" },
                new WtIcon { Class = "wtf-organization-o", Glyph = "\ue6a2" },
                new WtIcon { Class = "wtf-draft", Glyph = "\ue6a3" },
                new WtIcon { Class = "wtf-maybe", Glyph = "\ue6a4" },
                new WtIcon { Class = "wtf-accepted", Glyph = "\ue6a5" },
                new WtIcon { Class = "wtf-maybe-c", Glyph = "\ue6a6" },
                new WtIcon { Class = "wtf-accepted-c", Glyph = "\ue6a7" },
                new WtIcon { Class = "wtf-contract-add-o", Glyph = "\ue6a8" },
                new WtIcon { Class = "wtf-eye", Glyph = "\ue6a9" },
                new WtIcon { Class = "wtf-eye-slash", Glyph = "\ue6aa" },
                new WtIcon { Class = "wtf-processing", Glyph = "\ue6ab" },
                new WtIcon { Class = "wtf-completed", Glyph = "\ue6ac" },
                new WtIcon { Class = "wtf-pending", Glyph = "\ue6ad" },
                new WtIcon { Class = "wtf-type-ios", Glyph = "\ue725" },
                new WtIcon { Class = "wtf-type-worksheet", Glyph = "\ue726" },
                new WtIcon { Class = "wtf-type-task", Glyph = "\ue727" },
                new WtIcon { Class = "wtf-type-man", Glyph = "\ue728" },
                new WtIcon { Class = "wtf-type-pending", Glyph = "\ue729" },
                new WtIcon { Class = "wtf-type-message", Glyph = "\ue72a" },
                new WtIcon { Class = "wtf-type-pc", Glyph = "\ue72b" },
                new WtIcon { Class = "wtf-type-star", Glyph = "\ue72c" },
                new WtIcon { Class = "wtf-type-money", Glyph = "\ue72d" },
                new WtIcon { Class = "wtf-type-bug", Glyph = "\ue72e" },
                new WtIcon { Class = "wtf-type-demand", Glyph = "\ue72f" },
                new WtIcon { Class = "wtf-type-android", Glyph = "\ue730" },
                new WtIcon { Class = "wtf-template-task", Glyph = "\ue731" },
                new WtIcon { Class = "wtf-template-scrum", Glyph = "\ue732" },
                new WtIcon { Class = "wtf-template-personal", Glyph = "\ue733" },
                new WtIcon { Class = "wtf-template-kanban", Glyph = "\ue734" },
                new WtIcon { Class = "wtf-template-bug", Glyph = "\ue735" },
                new WtIcon { Class = "wtf-template-administration", Glyph = "\ue736" },
                new WtIcon { Class = "wtf-template-crm", Glyph = "\ue737" },
                new WtIcon { Class = "wtf-template-project", Glyph = "\ue738" },
                new WtIcon { Class = "wtf-step-division", Glyph = "\ue6ae" },
                new WtIcon { Class = "wtf-copy", Glyph = "\ue6b0" },
                new WtIcon { Class = "wtf-examine", Glyph = "\ue6af" },
                new WtIcon { Class = "wtf-angle-right", Glyph = "\ue739" },
                new WtIcon { Class = "wtf-schedule-o", Glyph = "\ue6b1" },
                new WtIcon { Class = "wtf-schedule", Glyph = "\ue6b2" },
                new WtIcon { Class = "wtf-dot-circle-o", Glyph = "\ue6b5" },
                new WtIcon { Class = "wtf-dot-circle", Glyph = "\ue6b6" },
                new WtIcon { Class = "wtf-angle-down-blod", Glyph = "\ue6b7" },
                new WtIcon { Class = "wtf-angle-down", Glyph = "\ue6b8" },
                new WtIcon { Class = "wtf-angle-up", Glyph = "\ue73a" },
                new WtIcon { Class = "wtf-table", Glyph = "\ue6b9" },
                new WtIcon { Class = "wtf-kanban", Glyph = "\ue6ba" },
                new WtIcon { Class = "wtf-list", Glyph = "\ue6bb" },
                new WtIcon { Class = "wtf-list-o", Glyph = "\ue6bc" },
                new WtIcon { Class = "wtf-kanban-o", Glyph = "\ue6bd" },
                new WtIcon { Class = "wtf-table-o", Glyph = "\ue6be" },
                new WtIcon { Class = "wtf-timeline-o", Glyph = "\ue6bf" },
                new WtIcon { Class = "wtf-insight-o", Glyph = "\ue6c0" },
                new WtIcon { Class = "wtf-iteration-o", Glyph = "\ue6c1" },
                new WtIcon { Class = "wtf-calendar-view-o", Glyph = "\ue6c2" },
                new WtIcon { Class = "wtf-iteration", Glyph = "\ue6c3" },
                new WtIcon { Class = "wtf-insight", Glyph = "\ue6c4" },
                new WtIcon { Class = "wtf-calendar-view", Glyph = "\ue6c5" },
                new WtIcon { Class = "wtf-timeline", Glyph = "\ue6c6" },
                new WtIcon { Class = "wtf-workload", Glyph = "\ue6c7" },
                new WtIcon { Class = "wtf-pause-circle", Glyph = "\ue6c8" },
                new WtIcon { Class = "wtf-completed-circle", Glyph = "\ue6c9" },
                new WtIcon { Class = "wtf-lock", Glyph = "\ue6ca" },
                new WtIcon { Class = "wtf-unlock", Glyph = "\ue6cb" },
                new WtIcon { Class = "wtf-times-circle", Glyph = "\ue6cc" },
                new WtIcon { Class = "wtf-columns", Glyph = "\ue6cd" },
                new WtIcon { Class = "wtf-exchange-alt", Glyph = "\ue6ce" },
                new WtIcon { Class = "wtf-bold", Glyph = "\ue6cf" },
                new WtIcon { Class = "wtf-check-square1", Glyph = "\ue6d0" },
                new WtIcon { Class = "wtf-code", Glyph = "\ue6d1" },
                new WtIcon { Class = "wtf-heading", Glyph = "\ue6d2" },
                new WtIcon { Class = "wtf-image1", Glyph = "\ue6d3" },
                new WtIcon { Class = "wtf-italic", Glyph = "\ue6d4" },
                new WtIcon { Class = "wtf-link1", Glyph = "\ue6d5" },
                new WtIcon { Class = "wtf-list-ul1", Glyph = "\ue6d6" },
                new WtIcon { Class = "wtf-long-arrow-alt-right", Glyph = "\ue6d7" },
                new WtIcon { Class = "wtf-list-ol", Glyph = "\ue6da" },
                new WtIcon { Class = "wtf-minus", Glyph = "\ue6db" },
                new WtIcon { Class = "wtf-quote-left", Glyph = "\ue6dc" },
                new WtIcon { Class = "wtf-sliders-h", Glyph = "\ue6dd" },
                new WtIcon { Class = "wtf-smile", Glyph = "\ue6de" },
                new WtIcon { Class = "wtf-square", Glyph = "\ue6df" },
                new WtIcon { Class = "wtf-strikethrough", Glyph = "\ue6e0" },
                new WtIcon { Class = "wtf-superscript", Glyph = "\ue6e1" },
                new WtIcon { Class = "wtf-table-toolbar", Glyph = "\ue6e2" },
                new WtIcon { Class = "wtf-underline", Glyph = "\ue6e3" },
                new WtIcon { Class = "wtf-expand-arrows-alt", Glyph = "\ue6e4" },
                new WtIcon { Class = "wtf-dashboard-circle", Glyph = "\ue6ea" },
                new WtIcon { Class = "wtf-iphone", Glyph = "\ue6e7" },
                new WtIcon { Class = "wtf-pc", Glyph = "\ue6e8" },
                new WtIcon { Class = "wtf-android", Glyph = "\ue6e9" },
                new WtIcon { Class = "wtf-unselected-o", Glyph = "\ue6eb" },
                new WtIcon { Class = "wtf-property-attachment", Glyph = "\ue6ec" },
                new WtIcon { Class = "wtf-property-tag", Glyph = "\ue6ed" },
                new WtIcon { Class = "wtf-property-priority", Glyph = "\ue6ee" },
                new WtIcon { Class = "wtf-property-iteration", Glyph = "\ue6ef" },
                new WtIcon { Class = "wtf-property-sub-task", Glyph = "\ue6f0" },
                new WtIcon { Class = "wtf-property-number", Glyph = "\ue6f1" },
                new WtIcon { Class = "wtf-property-text", Glyph = "\ue6f2" },
                new WtIcon { Class = "wtf-property-textarea", Glyph = "\ue6f3" },
                new WtIcon { Class = "wtf-property-multi-select", Glyph = "\ue6f4" },
                new WtIcon { Class = "wtf-relation-o", Glyph = "\ue684" },
                new WtIcon { Class = "wtf-pending-lg-o", Glyph = "\ue6f5" },
                new WtIcon { Class = "wtf-completed-lg-o", Glyph = "\ue6f6" },
                new WtIcon { Class = "wtf-relation-th-o", Glyph = "\ue6fa" },
                new WtIcon { Class = "wtf-workload-o", Glyph = "\ue6fb" },
                new WtIcon { Class = "wtf-workload-th-o", Glyph = "\ue6fc" },
                new WtIcon { Class = "wtf-begin-date-th-o", Glyph = "\ue6fd" },
                new WtIcon { Class = "wtf-processing-lg-o", Glyph = "\ue6f7" },
                new WtIcon { Class = "wtf-due-date-th-o", Glyph = "\ue6fe" },
                new WtIcon { Class = "wtf-user-add-o", Glyph = "\ue6f8" },
                new WtIcon { Class = "wtf-user-add-th-o", Glyph = "\ue6f9" },
                new WtIcon { Class = "wtf-begin-date-o", Glyph = "\ue6ff" },
                new WtIcon { Class = "wtf-due-date-o", Glyph = "\ue700" },
                new WtIcon { Class = "wtf-property-select", Glyph = "\ue702" },
                new WtIcon { Class = "wtf-property-date", Glyph = "\ue703" },
                new WtIcon { Class = "wtf-property-workload", Glyph = "\ue704" },
                new WtIcon { Class = "wtf-property-members", Glyph = "\ue705" },
                new WtIcon { Class = "wtf-property-member", Glyph = "\ue706" },
                new WtIcon { Class = "wtf-workload-analytic", Glyph = "\ue701" },
                new WtIcon { Class = "wtf-time-view", Glyph = "\ue707" },
                new WtIcon { Class = "wtf-analytic-insight", Glyph = "\ue708" },
                new WtIcon { Class = "wtf-my", Glyph = "\ue709" },
                new WtIcon { Class = "wtf-subordinate", Glyph = "\ue70a" },
                new WtIcon { Class = "wtf-project-setting", Glyph = "\ue73b" },
                new WtIcon { Class = "wtf-link-leave", Glyph = "\ue70b" },
                new WtIcon { Class = "wtf-mission", Glyph = "\ue70c" },
                new WtIcon { Class = "wtf-mission-o", Glyph = "\ue70d" },
                new WtIcon { Class = "wtf-connect", Glyph = "\ue70e" },
                new WtIcon { Class = "wtf-disconnect", Glyph = "\ue70f" }
            };
        }

        public static WtIcon[] Icons { get; }

        public static string GetGlyph(string @class)
        {
            return Icons.Single(i => i.Class == @class).Glyph;
        }

        public static string GetGlyph(WtVisibility visibility)
        {
            switch (visibility)
            {
                case WtVisibility.Public: return "\ue70c";
                case WtVisibility.Private: return "\ue667";
            }
            return null;
        }

        public static string GetBoldGlyph(int taskType)
        {
            string glyph = null;
            switch (taskType)
            {
                case 1: glyph = "\ue6ad"; break;
                case 2: glyph = "\ue6ab"; break;
                case 3: glyph = "\ue6ac"; break;
            }
            return glyph;
        }

        public static string GetLightGlyph(int taskType)
        {
            string glyph = null;
            switch (taskType)
            {
                case 1: glyph = "\ue6f5"; break;
                case 2: glyph = "\ue6f7"; break;
                case 3: glyph = "\ue6f6"; break;
            }
            return glyph;
        }

        public static string GetGlyphByRelationType(int type)
        {
            string glyph = null;
            switch (type)
            {
                case 1: glyph = "\ue66b"; break;
                case 2: glyph = "\ue70e"; break;
                case 3: glyph = "\ue700"; break;
            }
            return glyph;
        }

        public static string GetAppIcon(string app)
        {
            switch (app)
            {
                case "message": return "\ue618";
                case "task": return "\ue614";
                case "calendar": return "\ue619";
                case "drive": return "\ue616";
                case "report": return "\ue60c";
                case "crm": return "\ue605";
                case "approval": return "\ue602";
                case "bulletin": return "\ue60b";
                case "leave": return "\ue607";
                case "portal": return "\ue606";
                case "appraisal": return "\ue609";
                case "okr": return "\ue613";
                case "mission": return "\ue70d";
                default: return null;
            }
        }

        public static string GetAppIcon2(string app)
        {
            switch (app)
            {
                case "message": return "\ue61e";
                case "task": return "\ue61a";
                case "calendar": return "\ue615";
                case "drive": return "\ue61c";
                case "report": return "\ue60d";
                case "crm": return "\ue60f";
                case "approval": return "\ue611";
                case "bulletin": return "\ue60e";
                case "leave": return "\ue608";
                case "portal": return "\ue610";
                case "appraisal": return "\ue601";
                case "okr": return "\ue60a";
                case "mission": return "\ue70c";
                default: return null;
            }
        }
    }
}
