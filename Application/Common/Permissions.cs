using System.Reflection;

namespace Application.Common;

public static class Permissions
{
    // ================= PROJE =================
    public static class Proje
    {
        public const string View = "Proje.View";
        public const string Create = "Proje.Create";
        public const string Update = "Proje.Update";
        public const string Delete = "Proje.Delete";
    }

    // ================= KATEGORI =================
    public static class Kategori
    {
        public const string View = "Kategori.View";
        public const string Create = "Kategori.Create";
        public const string Update = "Kategori.Update";
        public const string Delete = "Kategori.Delete";
    }

    // ================= DEGER =================
    public static class Deger
    {
        public const string View = "Deger.View";
        public const string Create = "Deger.Create";
        public const string Update = "Deger.Update";
        public const string Delete = "Deger.Delete";
    }

    // ================= USER =================
    public static class User
    {
        public const string View = "User.View";
        public const string Create = "User.Create";
        public const string Update = "User.Update";
        public const string Delete = "User.Delete";
        public const string RoleAssign = "User.RoleAssign";
    }

    // ================= ROLE =================
    public static class Role
    {
        public const string View = "Role.View";
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";
        public const string PermissionAssign = "Role.PermissionAssign";
    }
    public static class AuditLog
    {
        public const string View = "AuditLog.View";
    }

    // ================= TÜM PERMISSIONLARI AL =================
    public static List<string> GetAll()
    {
        return typeof(Permissions)
            .GetNestedTypes()
            .SelectMany(t => t.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy))
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
            .Select(fi => fi.GetRawConstantValue()?.ToString())
            .Where(v => v != null)!
            .ToList();
    }

    // ================= GROUPLU AL (UI için) =================
    public static Dictionary<string, List<string>> GetGrouped()
    {
        return typeof(Permissions)
            .GetNestedTypes()
            .ToDictionary(
                t => t.Name,
                t => t.GetFields(
                        BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                    .Select(fi => fi.GetRawConstantValue()?.ToString()!)
                    .ToList()
            );
    }
}